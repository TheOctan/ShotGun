using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
	public event Action<float> OnMove
	{
		add { controller.OnMove += value; }
		remove { controller.OnMove -= value; }
	}

	public float moveSpeed = 5.0f;

	public Crosshairs crosshairs;

	private Camera viewCamera;
	private PlayerController controller;
	private GunController gunController;

	protected override void Start()
	{
		base.Start();
	}

	void Awake()
	{
		controller = GetComponent<PlayerController>();
		gunController = GetComponent<GunController>();
		viewCamera = Camera.main;
		FindObjectOfType<Spawner>().OnNewWave += OnNewWave;
	}

	void OnNewWave(int waveNumber)
	{
		health = startingHealth;
		gunController.EquipGun(waveNumber - 1);
	}

	void Update()
	{
		if (GameUI.GameIsPaused)
			return;

		// Movement input
		Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move(moveVelocity);

		// Look input
		Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunController.GunHeight);
		float rayDistance;

		if (groundPlane.Raycast(ray, out rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			Debug.DrawLine(ray.origin, point, Color.red);
			controller.LookAt(point);
			crosshairs.transform.position = point;
			crosshairs.DetectTargets(ray);

			if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > 1)
			{
				gunController.Aim(point);
			}
		}

		// Weapon input
		if (Input.GetMouseButton(0))
		{
			gunController.OnTriggerHold();
		}
		if (Input.GetMouseButtonUp(0))
		{
			gunController.OnTriggerRelease();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			gunController.Reload();
		}

		if(transform.position.y < -10)
		{
			TakeDamage(health);
		}
	}

	protected override void Die()
	{
		AudioManager.instance.PlaySound("Player Death", transform.position);
		base.Die();
	}
}