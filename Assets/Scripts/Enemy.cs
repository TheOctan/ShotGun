using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
	public enum State
	{
		Idle = 0,
		Chasing,
		Attacking
	}
	private State currentState;

	public ParticleSystem deathEffect;
	public static event System.Action OnDeathStatic;

	private NavMeshAgent pathFinder;
	private Transform target;
	private LivingEntity targetEntity;
	private Material skinMaterial;

	private Color originalColor;

	private float attackDistanceThreshold = 0.5f;
	private float timeBetweenAttacks = 1f;

	private float nextAttackTime;
	private float myCollisionRadius;
	private float targetCollisionRadius;

	private bool hasTarget;
	private float damage = 1;

	private void Awake()
	{
		pathFinder = GetComponent<NavMeshAgent>();

		var player = GameObject.FindGameObjectWithTag("Player");
		if (player != null)
		{
			hasTarget = true;

			target = player.transform;
			targetEntity = player.GetComponent<LivingEntity>();

			myCollisionRadius = GetComponent<CapsuleCollider>().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
		}
	}

	protected override void Start()
	{
		base.Start();

		if(hasTarget)
		{
			currentState = State.Chasing;
			targetEntity.OnDeath += OnTargetDeath;

			StartCoroutine(UpdatePath());
		}
	}

	public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColor)
	{
		pathFinder.speed = moveSpeed;

		if (hasTarget)
		{
			damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
		}
		startingHealth = enemyHealth;

		skinMaterial = GetComponent<Renderer>().sharedMaterial;
		skinMaterial.color = skinColor;
		originalColor = skinMaterial.color;
	}

	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		AudioManager.instance.PlaySound("Impact", transform.position);
		if(damage >= health)
		{
			OnDeathStatic?.Invoke();
			AudioManager.instance.PlaySound("Enemy Death", transform.position);
			Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)), deathEffect.main.startLifetime.constant);
		}
		base.TakeHit(damage, hitPoint, hitDirection);
	}

	void OnTargetDeath()
	{
		hasTarget = false;
		currentState = State.Idle;
	}

	void Update()
	{
		if (hasTarget)
		{
			if (Time.time > nextAttackTime)
			{
				float sqrDistToTarget = (target.position - transform.position).sqrMagnitude;

				if (sqrDistToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
				{
					nextAttackTime = Time.time + timeBetweenAttacks;
					AudioManager.instance.PlaySound("Enemy Attack", transform.position);
					StartCoroutine(Attack());
				}
			}
		}
	}

	IEnumerator Attack()
	{
		currentState = State.Attacking;
		pathFinder.enabled = false;

		Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius/* + targetCollisionRadius*/);
		
		float attackSpeed = 3;
		float percent = 0;

		skinMaterial.color = Color.red;
		bool hasAppliedDamage = false;

		while (percent <= 1)
		{
			if(percent >= 0.5f && !hasAppliedDamage)
			{
				hasAppliedDamage = true;
				targetEntity.TakeDamage(damage);
			}

			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
			transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

			yield return null;
		}

		skinMaterial.color = originalColor;
		currentState = State.Chasing;
		pathFinder.enabled = true;
	}

	IEnumerator UpdatePath()
	{
		float refreshRate = 0.25f;

		while (hasTarget)
		{
			if (currentState == State.Chasing)
			{
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
				if (!dead)
				{
					pathFinder.SetDestination(targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}
