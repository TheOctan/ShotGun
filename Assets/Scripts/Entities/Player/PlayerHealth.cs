using OctanGames.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.Entities.Player
{
	public class PlayerHealth : LivingEntity
	{
		[SerializeField] private float deadHeight = -10;

		private void Update()
		{
			CheckHeight();
		}

		public void OnNewWave()
		{
			Health = startingHealth;
		}

		protected override void Die()
		{
			base.Die();
			AudioManager.instance.PlaySound("Player Death", transform.position);
		}

		private void CheckHeight()
		{
			if (transform.position.y < deadHeight)
			{
				TakeDamage(Health);
			}
		}
	}
}