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

	[Header("Recoil")]
	public Vector2 kickMinMax = new Vector2(0.05f, 0.2f);
	public Vector2 recoilAngleMinMax = new Vector2(3, 5);
	public float recoilMoveSettleTime = 0.1f;
	public float recoilRotationSettleTime = 0.1f;

	[Header("Effects")]
	public Transform shell;
	public Transform shellEjection;

	private Muzzleflash muzzleflash;
	private float nextShotTime;

	private bool triggerReleaseSinceLastShot;
	private int shotsRemaningInBurst;

	private Vector3 recoilSmootDampVelocity;
	private float recoilRotSmoothDampVelocity;
	private float recoilAngle;

	void Start()
	{
		muzzleflash = GetComponent<Muzzleflash>();
		shotsRemaningInBurst = burstCount;
	}

	void LateUpdate()
	{
		// animate recoil
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmootDampVelocity, recoilMoveSettleTime);
		recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
		transform.localEulerAngles = Vector3.left * recoilAngle;
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
			transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
			recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
			recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
		}
	}

	public void Aim(Vector3 aimPoint)
	{
		transform.LookAt(aimPoint);
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
