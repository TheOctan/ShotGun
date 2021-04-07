using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Experimental
{
	public class PlayerMover : LivingEntity
	{
		[Header("Components")]
		[SerializeField] private Legacy.PlayerController controller;
		[SerializeField] private GunController gunController;
		[SerializeField] private Crosshairs crosshairs;
		[SerializeField] private PlayerInput playerInput;

		[Header("Properties")]
		[SerializeField, Min(0)] private float aimRadius = 3f;
		[SerializeField, Min(0)] private float moveSpeed = 5.0f;

		private Camera viewCamera;
		private Vector2 moveDirection;
		private Vector3 lastAimPosition;

		private string currentControlScheme;
		private bool isFire;
		private bool isGamepad;
		private int currentGun = 0;

		private void Awake()
		{
			currentControlScheme = playerInput.currentControlScheme;
			lastAimPosition = transform.forward * aimRadius;
		}
		protected override void Start()
		{
			base.Start();
			viewCamera = Camera.main;
			gunController.EquipGun(currentGun);
		}
		private void Update()
		{
			if (isFire)
			{
				gunController.OnTriggerHold();
			}

			Move(moveDirection);

			Vector3 point;
			if (isGamepad)
			{
				point = AnalogAim();
			}
			else
			{
				point = AimCrosshairs();
			}

			AimGun(point);
			controller.LookAt(point);
			crosshairs.transform.position = point;

			CheckHeight();
		}

		public void OnChangeSheme()
		{
			if (playerInput.currentControlScheme != currentControlScheme)
			{
				currentControlScheme = playerInput.currentControlScheme;
				if (currentControlScheme == "Gamepad")
				{
					isGamepad = true;
				}
				else if (currentControlScheme == "Keyboard And Mouse")
				{
					isGamepad = false;
				}
			}
		}
		public void SelectWeapon(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				int delta = (int)context.ReadValue<float>();
				currentGun = Mathf.Clamp(currentGun + delta, 0, gunController.GunCount - 1);
				gunController.EquipGun(currentGun);
			}
		}
		public void OnMovePlayer(InputAction.CallbackContext context)
		{
			moveDirection = context.ReadValue<Vector2>();
		}
		public void OnLookPlayer(InputAction.CallbackContext context)
		{
			Vector2 aim = context.ReadValue<Vector2>();
			Vector3 lookPosition = new Vector3(aim.x, 0, aim.y);

			if (lookPosition != Vector3.zero)
			{
				lastAimPosition = lookPosition.normalized * aimRadius;
			}
		}
		public void OnFire(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				isFire = true;
				//if (currentControlScheme == "Keyboard And Mouse")
				//{
				//	isGamepad = false;
				//}
			}
			else if (context.canceled)
			{
				isFire = false;
				gunController.OnTriggerRelease();
			}
		}
		public void OnReload(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				gunController.Reload();
			}
		}

		private void Move(Vector2 direction)
		{
			Vector3 targetDirectory = new Vector3(direction.x, 0, direction.y);
			Vector3 moveVelocity = targetDirectory * moveSpeed;
			controller.Move(moveVelocity);
		}

		private Vector3 AimCrosshairs()
		{
			Vector3 mousePosition = Mouse.current.position.ReadValue();
			Ray ray = viewCamera.ScreenPointToRay(mousePosition);
			Plane groundPlane = new Plane(Vector3.up, Vector3.up * 0.3f);

			if (groundPlane.Raycast(ray, out float rayDistance))
			{
				Vector3 point = ray.GetPoint(rayDistance);
				Debug.DrawLine(ray.origin, point, Color.red);

				crosshairs.DetectTargets(ray);

				return point;
			}

			return Vector3.zero;
		}

		private Vector3 AnalogAim()
		{
			return transform.position + lastAimPosition;
		}

		private void AimGun(Vector3 point)
		{
			if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > 1)
			{
				gunController.Aim(point);
			}
		}

		private void CheckHeight()
		{
			if (transform.position.y < -10)
			{
				TakeDamage(health);
			}
		}
	}
}