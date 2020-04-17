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
	public int projectilesPerMag;
	public float reloadTime = 0.3f;

	[Header("Recoil")]
	public Vector2 kickMinMax = new Vector2(0.05f, 0.2f);
	public Vector2 recoilAngleMinMax = new Vector2(3, 5);
	public float recoilMoveSettleTime = 0.1f;
	public float recoilRotationSettleTime = 0.1f;

	[Header("Effects")]
	public Transform shell;
	public Transform shellEjection;
	public AudioClip shootAudio;
	public AudioClip reloadAudio;

	private MuzzleFlash muzzleflash;
	private float nextShotTime;

	private bool triggerReleasedSinceLastShot;
	private int shotsRemainingInBurst;
	private int projectilesRemaningInMag;
	private float reloadAngle;
	private bool isReloading;

	private Vector3 recoilSmootDampVelocity;
	private float recoilRotSmoothDampVelocity;
	private float recoilAngle;

	void Start()
	{
		muzzleflash = GetComponent<MuzzleFlash>();
		shotsRemainingInBurst = burstCount;
		projectilesRemaningInMag = projectilesPerMag;
	}

	void LateUpdate()
	{
		// animate recoil
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmootDampVelocity, recoilMoveSettleTime);
		recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
		transform.localEulerAngles = Vector3.left * (recoilAngle + reloadAngle) + Vector3.up * transform.localEulerAngles.y;

		if (!isReloading && projectilesRemaningInMag == 0)
		{
			Reload();
		}
	}

	void Shoot()
	{
		if (!isReloading && Time.time > nextShotTime && projectilesRemaningInMag > 0)
		{
			if (fireMode == FireMode.Burst)
			{
				if (shotsRemainingInBurst == 0)
				{
					return;
				}
				shotsRemainingInBurst--;
			}
			else if (fireMode == FireMode.Single)
			{
				if (!triggerReleasedSinceLastShot)
				{
					return;
				}
			}

			for (int i = 0; i < projectileSpawn.Length; i++)
			{
				if (projectilesRemaningInMag == 0)
				{
					break;
				}
				projectilesRemaningInMag--;
				nextShotTime = Time.time + msBetweenShots / 1000f;
				Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation);
				newProjectile.SetSpeed(muzzleVelocity);
			}

			Instantiate(shell, shellEjection.position, shellEjection.rotation);
			muzzleflash.Activate();
			transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
			recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
			recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

			AudioManager.instance.PlaySound(shootAudio, transform.position);
		}
	}

	public void Reload()
	{
		if (!isReloading && projectilesRemaningInMag != projectilesPerMag)
		{
			StartCoroutine(AnimateReload());
			AudioManager.instance.PlaySound(reloadAudio, transform.position);
		}
	}

	IEnumerator AnimateReload()
	{
		isReloading = true;
		yield return new WaitForSeconds(0.2f);

		float reloadSpeed = 1f / reloadTime;
		float percent = 0;
		float maxReloadAngle = 30;
		Vector3 initialRot = transform.localEulerAngles;

		while (percent < 1)
		{
			percent += Time.deltaTime * reloadSpeed;
			float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
			reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
			transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

			yield return null;
		}

		isReloading = false;
		projectilesRemaningInMag = projectilesPerMag;
	}

	public void Aim(Vector3 aimPoint)
	{
		if (!isReloading)
		{
			transform.LookAt(aimPoint);
		}
	}

	public void OnTriggerHold()
	{
		Shoot();
		triggerReleasedSinceLastShot = false;
	}

	public void OnTriggerRelease()
	{
		triggerReleasedSinceLastShot = true;
		shotsRemainingInBurst = burstCount;
	}
}
