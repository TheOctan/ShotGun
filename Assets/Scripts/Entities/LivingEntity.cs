using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OctanGames.Entities
{
    public abstract class LivingEntity : MonoBehaviour, IDamageable
    {
        public float startingHealth = 3;
        public float Health { get; protected set; }
        protected bool isDead;

        [Header("Events")]
        public DeathEvent DeathEvent;
        public HitDamageEvent HitDamageEvent;

        protected virtual void Start()
        {
            Health = startingHealth;
        }

        public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
        {
            TakeDamage(damage);
        }

        public virtual void TakeDamage(float damage)
        {
            Health -= damage;
            HitDamageEvent.Invoke(damage);

            if (Health <= 0 && !isDead)
            {
                Die();
            }
        }

        [ContextMenu("Self Destruct")]
        protected virtual void Die()
        {
            isDead = true;
            DeathEvent.Invoke();
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class DeathEvent : UnityEvent
    {
    }

    [System.Serializable]
    public class HitDamageEvent : UnityEvent<float>
    {
    }
}