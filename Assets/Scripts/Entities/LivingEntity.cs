using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 3;
    public float Health { get; protected set; }
    protected bool isDead;

    public event Action OnDeath;
	public event Action<float> OnHitDamage;

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
        OnHitDamage?.Invoke(damage);

        if (Health <= 0 && !isDead)
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    protected virtual void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
