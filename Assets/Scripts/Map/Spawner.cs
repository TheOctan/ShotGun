﻿using OctanGames.Entities;
using OctanGames.Entities.Player;
using OctanGames.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
	public bool devMode;

	public Wave[] waves;
	public Enemy enemy;

	[Header("Events")]
	public NewWaveEvent NewWaveEvent;

	private LivingEntity playerEntity;
	private Transform playerT;

	private Wave currentWave;
	private int currentWaveNumber = 0;

	private int enemiesRemainingToSpawn;
	private int enemiesRemainingAlive;
	private float nextSpawnTime;

	private MapGenerator map;

	private float timeBetweenCampingChecks = 2;
	private float campThresholdDistance = 1.5f;
	private float nextCampCheckTime;
	private Vector3 campPositionOld;
	private bool isCamping;

	private bool isDisabled;
	
	private void Start()
	{
		playerEntity = FindObjectOfType<PlayerHealth>();
		playerT = playerEntity.transform;

		nextCampCheckTime = timeBetweenCampingChecks + Time.time;
		campPositionOld = playerT.position;
		playerEntity.DeathEvent.AddListener(OnPlayerDeath);

		map = FindObjectOfType<MapGenerator>();
		NextWave();
	}

	private void Update()
	{
		if (!isDisabled)
		{
			if (Time.time > nextCampCheckTime)
			{
				nextCampCheckTime = Time.time + timeBetweenCampingChecks;

				isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
				campPositionOld = playerT.position;
			}

			if ((enemiesRemainingToSpawn > 0 || currentWave.infinit) && Time.time > nextSpawnTime)
			{
				enemiesRemainingToSpawn--;
				nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

				StartCoroutine("SpawnEnemy");
			}
		}

		if (devMode)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				StopCoroutine("SpawnEnemy");
				foreach (var enemy in FindObjectsOfType<Enemy>())
				{
					Destroy(enemy.gameObject);
				}
				NextWave();
			}
		}
	}

	IEnumerator SpawnEnemy()
	{
		float spawnDelay = 1;
		float tileFlashSpeed = 4;

		Transform spawntile = map.GetRandomTile();
		if (isCamping)
		{
			spawntile = map.GetTileFromPosition(playerT.position);
		}
		Material tileMat = spawntile.GetComponent<Renderer>().material;
		Color initialColor = Color.white;
		Color flashColor = Color.red;
		float spawnTimer = 0;

		while (spawnTimer < spawnDelay)
		{
			tileMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

			spawnTimer += Time.deltaTime;
			yield return null;
		}

		Enemy spawnedEnemy = Instantiate(enemy, spawntile.position + Vector3.up, Quaternion.identity);
		spawnedEnemy.DeathEvent.AddListener(OnEnemyDeath);
		spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColor);
	}

	void OnPlayerDeath()
	{
		isDisabled = true;
	}

	void OnEnemyDeath()
	{
		enemiesRemainingAlive--;

		if (enemiesRemainingAlive == 0)
		{
			NextWave();
		}
	}

	void ResetPlayerPosition()
	{
		playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
	}

	void NextWave()
	{
		if(currentWaveNumber > 0)
		{
			AudioManager.instance.PlaySound2D("Level Complete");
		}

		currentWaveNumber++;
		if (currentWaveNumber - 1 < waves.Length)
		{
			currentWave = waves[currentWaveNumber - 1];

			enemiesRemainingToSpawn = currentWave.enemyCount;
			enemiesRemainingAlive = enemiesRemainingToSpawn;

			NewWaveEvent.Invoke(currentWaveNumber);

			ResetPlayerPosition();
		}
	}

	[System.Serializable]
	public class Wave
	{
		public bool infinit;
		public int enemyCount;
		public float timeBetweenSpawns;

		public float moveSpeed;
		public int hitsToKillPlayer;
		public float enemyHealth;
		public Color skinColor;
	}
}

[System.Serializable]
public class NewWaveEvent : UnityEvent<int>
{
}