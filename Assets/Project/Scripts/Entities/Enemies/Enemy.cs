using OctanGames.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace OctanGames.Entities
{
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
		public static event Action OnDeathStatic;
		public static event Action<float> OnHitDamageStatic;

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
		private CapsuleCollider _capsuleCollider;

		public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColor)
		{
			pathFinder.speed = moveSpeed;

			if (hasTarget)
			{
				damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
			}
			startingHealth = enemyHealth;

			var main = deathEffect.main;
			main.startColor = new Color(skinColor.r, skinColor.g, skinColor.b, 1);

			skinMaterial = GetComponent<Renderer>().material;
			skinMaterial.color = skinColor;
			originalColor = skinMaterial.color;
		}
		public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
		{
			AudioManager.instance.PlaySound("Impact", transform.position);
			if (damage >= Health && !isDead)
			{
				OnDeathStatic?.Invoke();
				AudioManager.instance.PlaySound("Enemy Death", transform.position);
				Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)), deathEffect.main.startLifetime.constant);
			}
			base.TakeHit(damage, hitPoint, hitDirection);
			OnHitDamageStatic?.Invoke(damage);

		}

		private void Awake()
		{
			pathFinder = GetComponent<NavMeshAgent>();

			var player = GameObject.FindGameObjectWithTag("Player");
			if (player != null)
			{
				hasTarget = true;

				target = player.transform;
				targetEntity = player.GetComponent<LivingEntity>();

				_capsuleCollider = GetComponent<CapsuleCollider>();
				myCollisionRadius = _capsuleCollider.radius;

				var playerCapsuleCollider = target.GetComponent<CapsuleCollider>();
				targetCollisionRadius = playerCapsuleCollider.radius;
			}
		}
		protected override void Start()
		{
			base.Start();

			if (hasTarget)
			{
				currentState = State.Chasing;
				targetEntity.DeathEvent.AddListener(OnTargetDeath);

				StartCoroutine(UpdatePath());
			}
		}
		private void Update()
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

		private void OnTargetDeath()
		{
			hasTarget = false;
			currentState = State.Idle;
		}
		private IEnumerator Attack()
		{
			currentState = State.Attacking;
			pathFinder.enabled = false;

			Vector3 originalPosition = transform.position;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			Vector3 attackPosition = target.position - dirToTarget * myCollisionRadius/* + targetCollisionRadius*/;

			float attackSpeed = 3;
			float percent = 0;

			skinMaterial.color = Color.red;
			bool hasAppliedDamage = false;

			while (percent <= 1)
			{
				if (percent >= 0.5f && !hasAppliedDamage)
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
		private IEnumerator UpdatePath()
		{
			float refreshRate = 0.25f;

			while (hasTarget)
			{
				if (currentState == State.Chasing)
				{
					Vector3 dirToTarget = (target.position - transform.position).normalized;
					Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
					if (!isDead)
					{
						pathFinder.SetDestination(targetPosition);
					}
				}
				yield return new WaitForSeconds(refreshRate);
			}
		}
	}
}