﻿using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionData sessionData { get; private set; }

	private Spawner spawner;
    private Player player;
    private Gun gun;

	private void Awake()
	{
		sessionData = new SessionData();
	}

	private void Start()
	{
		spawner = FindObjectOfType<Spawner>();
		player = FindObjectOfType<Player>();
		gun = FindObjectOfType<Gun>();

		SubscribeEvents();

		sessionData.Date = DateTime.Now.ToString("dd/MM/yyyy");
	}

	private void SubscribeEvents()
	{
		Enemy.OnHitDamageStatic += OnShootDamage;

		spawner.OnNewWave += OnNewWave;

		player.OnDeath += OnPlayerDeath;
		player.OnHitDamage += OnHitDamage;
		player.OnMove += OnPlayerMove;

		SubscribeGunEvents();
	}	

	private void SubscribeGunEvents()
	{
		gun.OnShoot += OnGunShoot;
		gun.OnReload += OnGunReload;
	}	
	private void UnsubscribeEvents()
	{
		Enemy.OnHitDamageStatic -= OnShootDamage;

		spawner.OnNewWave -= OnNewWave;

		gun.OnShoot -= OnGunShoot;
		gun.OnReload -= OnGunReload;
	}
	
	private void OnNewWave(int waveNumber)
	{
		gun = FindObjectOfType<Gun>();
		SubscribeGunEvents();
	}
	private void OnGunReload()
	{
		sessionData.ReloadCount++;
	}
	private void OnGunShoot()
	{
		sessionData.ShootCount++;
	}
	private void OnShootDamage(float damage)
	{
		sessionData.ShootDamage += damage;
	}
	private void OnHitDamage(float damage)
	{
		sessionData.HitDamage += damage;
	}
	private void OnPlayerMove(float distance)
	{
		sessionData.TraveledDistance += distance;
	}
	private void OnPlayerDeath()
	{
		UnsubscribeEvents();

		sessionData.Duration = Mathf.Floor(Time.timeSinceLevelLoad);
		sessionData.Score = ScoreKeeper.score;
	}
}
