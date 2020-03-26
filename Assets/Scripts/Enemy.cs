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

	protected override void Start()
	{
		base.Start();
		pathFinder = GetComponent<NavMeshAgent>();
		skinMaterial = GetComponent<Renderer>().material;
		originalColor = skinMaterial.color;

		var player = GameObject.FindGameObjectWithTag("Player");

		if(player != null)
		{
			currentState = State.Chasing;
			hasTarget = true;

			target = player.transform;
			targetEntity = player.GetComponent<LivingEntity>();
			targetEntity.OnDeath += OnTargetDeath;

			myCollisionRadius = GetComponent<CapsuleCollider>().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

			StartCoroutine(UpdatePath());
		}		
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
