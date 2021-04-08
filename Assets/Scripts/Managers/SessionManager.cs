using Assets.Scripts.Data;
using Assets.Scripts.Legacy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
	public static SessionData SessionData { get; private set; } = new SessionData();

	private Spawner spawner;
    //private Player player;
    private Gun gun;

	private void Start()
	{
		spawner = FindObjectOfType<Spawner>();
		//player = FindObjectOfType<Player>();
		gun = FindObjectOfType<Gun>();

		SubscribeEvents();

		SessionData.Date = DateTime.Now;
	}

	private void Update()
	{
		SessionData.Duration = Mathf.Floor(Time.timeSinceLevelLoad);
	}

	private void SubscribeEvents()
	{
		Enemy.OnHitDamageStatic += OnShootDamage;

		//spawner.OnNewWave += OnNewWave;

		//player.OnDeath += OnPlayerDeath;
		//player.OnHitDamage += OnHitDamage;
		//player.OnMove += OnPlayerMove;

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

		//spawner.OnNewWave -= OnNewWave;

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
		SessionData.ReloadCount++;
	}
	private void OnGunShoot()
	{
		SessionData.ShotCount++;
	}
	private void OnShootDamage(float damage)
	{
		SessionData.ShotDamage += damage;
	}
	private void OnHitDamage(float damage)
	{
		SessionData.HitDamage += damage;
	}
	private void OnPlayerMove(float distance)
	{
		SessionData.TraveledDistance += distance;
	}
	private void OnPlayerDeath()
	{
		UnsubscribeEvents();		
		SessionData.Score = ScoreKeeper.score;
	}
}
