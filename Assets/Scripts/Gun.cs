using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public enum FireMode
	{
		Auto = 0,
		Burst,
		Single
	}
	public FireMode fireMode;

	public Transform[] projectileSpawn;
	public Projectile projectile;
	public float msBetweenShots = 100f;
	public float muzzleVelocity = 35f;
	public int burstCount;

	public Transform shell;
	public Transform shellEjection;

	private Muzzleflash muzzleflash;
	private float nextShotTime;

	private bool triggerReleaseSinceLastShot;
	private int shotsRemaningInBurst;

	void Start()
	{
		muzzleflash = GetComponent<Muzzleflash>();
		shotsRemaningInBurst = burstCount;
	}

	void Shoot()
	{
		if (Time.time > nextShotTime)
		{
			switch (fireMode)
			{
				case FireMode.Auto:
					break;
				case FireMode.Burst:
					if (shotsRemaningInBurst == 0)
					{
						return;
					}
					shotsRemaningInBurst--;
					break;
				case FireMode.Single:
					if (!triggerReleaseSinceLastShot)
					{
						return;
					}
					break;
				default:
					break;
			}

			for (int i = 0; i < projectileSpawn.Length; i++)
			{
				nextShotTime = Time.time + msBetweenShots / 1000f;

				Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation);
				newProjectile.SetSpeed(muzzleVelocity);
			}

			Instantiate(shell, shellEjection.position, shellEjection.rotation);
			muzzleflash.Activate();
		}
	}

	public void OnTriggerHold()
	{
		Shoot();
		triggerReleaseSinceLastShot = false;
	}

	public void OnTriggerRelease()
	{
		triggerReleaseSinceLastShot = true;
		shotsRemaningInBurst = burstCount;
	}
}
