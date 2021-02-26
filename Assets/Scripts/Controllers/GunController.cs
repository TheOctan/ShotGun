using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
	public float GunHeight => weaponHold.position.y;
	public int GunCount => allGuns.Length;

	public Transform weaponHold;
	public Gun[] allGuns;
	private Gun equippedGun;

	public void EquipGun(Gun gunToEquip)
	{
		if (equippedGun != null)
		{
			Destroy(equippedGun.gameObject);
		}

		equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
		equippedGun.transform.parent = weaponHold;
	}

	public void EquipGun(int weaponIndex)
	{
		if (allGuns.Length != 0)
		{
			EquipGun(allGuns[weaponIndex]);
		}
	}

	public void OnTriggerHold()
	{
		equippedGun?.OnTriggerHold();
	}

	public void OnTriggerRelease()
	{
		equippedGun?.OnTriggerRelease();
	}

	public void Aim(Vector3 aimPoint)
	{
		equippedGun?.Aim(aimPoint);
	}

	public void Reload()
	{
		equippedGun?.Reload();
	}
}
