using Assets.Scripts.Legacy;
using OctanGames.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.Managers
{
	public class ScoreKeeperManager : MonoBehaviour
	{
		public static int score { get; private set; }

		private float lastEnemyKillTime;
		private int streakCount;
		private float streakExpiryTime = 1;

		void Start()
		{
			score = 0;
			Enemy.OnDeathStatic += OnEnemyKilled;
		}

		void OnEnemyKilled()
		{
			if (Time.time < lastEnemyKillTime + streakExpiryTime)
			{
				streakCount++;
			}
			else
			{
				streakCount = 0;
			}

			lastEnemyKillTime = Time.time;

			score += 3 + 2 * streakCount;
		}

		public void OnPlayerDeath()
		{
			Enemy.OnDeathStatic -= OnEnemyKilled;
		}
	}
}