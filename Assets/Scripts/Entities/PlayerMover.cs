using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PlayerMover : LivingEntity
{
	public float moveSpeed = 5.0f;

	[SerializeField] private PlayerController controller;
	[SerializeField] private GunController gunController;
	[SerializeField] private Crosshairs crosshairs;

	private InputMaster input;
	private Camera viewCamera;

	[Header("Aim")]
	[SerializeField] private float aimRadius = 2f;
	private Vector3 lastAimPosition;
	private bool isGamepad = false;

	private void Awake()
	{
		viewCamera = Camera.main;
		input = new InputMaster();

		gunController.EquipGun(0);

		input.Player.Fire.performed += _ => gunController.OnTriggerHold();
		input.Player.Release.performed += _ => gunController.OnTriggerRelease();
		input.Player.Reload.performed += _ => gunController.Reload();
	}

	private void OnEnable()
	{
		input.Player.Enable();
	}

	private void OnDisable()
	{
		input.Player.Disable();
	}

	protected override void Start()
	{
		base.Start();
	}

	private void Update()
	{
		Vector2 rawInput = input.Player.Move.ReadValue<Vector2>();
		Move(rawInput);

		Vector3 point = AnalogAim();

		controller.LookAt(point);
		crosshairs.transform.position = point;

		if (transform.position.y < -10)
		{
			TakeDamage(health);
		}
	}

	private void Move(Vector2 direction)
	{
		Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
		Vector3 moveVelocity = moveDirection * moveSpeed;
		controller.Move(moveVelocity);
	}

	private Vector3 AimCrosshair()
	{
		Vector3 mousePosition = Mouse.current.position.ReadValue();

		Ray ray = viewCamera.ScreenPointToRay(mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.up * 0.3f);
		float rayDistance;

		if (groundPlane.Raycast(ray, out rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			Debug.DrawLine(ray.origin, point, Color.red);

			crosshairs.DetectTargets(ray);

			if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > 1)
			{
				gunController.Aim(point);
			}

			return point;
		}

		return Vector3.zero;
	}

	private Vector3 AnalogAim()
	{
		Vector2 aim = input.Player.Look.ReadValue<Vector2>();
		Vector3 lookPosition = new Vector3(aim.x, 0, aim.y);

		if (lookPosition != Vector3.zero)
		{
			lastAimPosition = lookPosition.normalized * aimRadius;
		}

		return transform.position + lastAimPosition;
	}
}
