using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.Entities
{
	public interface IDamageable
	{
		void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection);
		void TakeDamage(float damage);
	}
}