using OctanGames.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("Sub Behaviours")]
	public MovementController movementController;
	public GunController gunController;

	[Header("Input Settings")]
	public PlayerInput playerInput;
	[SerializeField] private float aimHeight;
	[SerializeField] private float minAimRadius = 2f;
	[SerializeField] private bool useAimDistance = true;
	[SerializeField, Min(0)] private float aimDistance = 2f;

	public AimEvent aimEvent;
	public DetectEvent detectEvent;

	private Camera viewCamera;
	private Vector3 rawInputMovement;
	private Vector3 rotationDirection;
	private int currentGun = 0;
	private bool isFire;

	private string currentControlScheme;
	private const string gamepadControlScheme = "Gamepad";
	private const string keyboardControlScheme = "Keyboard And Mouse";

	public void OnControlsChanged()
	{
		Debug.LogWarning("With reloading scene, this method is no longer called.\nThis issue: https://forum.unity.com/threads/controls-changed-event-not-invoking-on-scene-change.885592/");
		if (playerInput.currentControlScheme != currentControlScheme)
		{
			currentControlScheme = playerInput.currentControlScheme;
		}
	}
	public void OnMovePlayer(InputAction.CallbackContext context)
	{
		Vector2 inputDirection = context.ReadValue<Vector2>();
		rawInputMovement = new Vector3(inputDirection.x, 0, inputDirection.y);
	}
	public void OnAnalogAimPlayer(InputAction.CallbackContext context)
	{
		Vector2 inputRotation = context.ReadValue<Vector2>();
		rotationDirection = new Vector3(inputRotation.x, 0, inputRotation.y);
	}
	public void OnFire(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			isFire = true;
		}
		else if (context.canceled)
		{
			isFire = false;
		}
	}
	public void OnReload(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			gunController.Reload();
		}
	}
	public void OnSelectWeapon(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			int delta = (int)context.ReadValue<float>();
			currentGun = Mathf.Clamp(currentGun + delta, 0, gunController.GunCount - 1);
			gunController.EquipGun(currentGun);
		}
	}

	public void AimGun(Vector3 point)
	{
		var aimPoint = new Vector2(point.x, point.z);
		var positionPoint = new Vector2(transform.position.x, transform.position.z);

		if ((aimPoint - positionPoint).sqrMagnitude > Mathf.Pow(minAimRadius, 2))
		{
			gunController.Aim(point);
		}
	}
	public void OnNewWave(int waveNumber)
	{
		gunController.EquipGun(waveNumber - 1);
	}

	private void Awake()
	{
		currentControlScheme = playerInput.currentControlScheme;
		aimEvent.AddListener(AimGun);
	}
	private void Start()
	{
		viewCamera = Camera.main;
		gunController.EquipGun(currentGun);
	}
	private void Update()
	{
		UpdateControlScheme();
		UpdatePlayerMovement();
		UpdatePlayerAttack();
		switch (currentControlScheme)
		{
			case keyboardControlScheme: UpdateMouseAim(); break;
			case gamepadControlScheme: UpdateAnalogAim(); break;
		}
	}

	private void UpdatePlayerMovement()
	{
		movementController.SetDirection(rawInputMovement);
	}
	private void UpdateMouseAim()
	{
		Vector3 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = viewCamera.ScreenPointToRay(mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.up * aimHeight);

		if (groundPlane.Raycast(ray, out float rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			Debug.DrawLine(ray.origin, point, Color.red);

			aimEvent.Invoke(point);
			detectEvent.Invoke(ray);

			movementController.LookAt(point);
		}
	}
	private void UpdateAnalogAim()
	{
		Vector3 aimPoint = transform.position + (transform.forward * minAimRadius);
		if (useAimDistance)
		{
			aimPoint += (rotationDirection * aimDistance);
		}
		aimPoint.y = aimHeight;

		Vector3 startPoint = viewCamera.transform.position;
		Vector3 rayDirection = aimPoint - startPoint;

		Ray ray = new Ray(startPoint, rayDirection);
		Debug.DrawLine(ray.origin, aimPoint, Color.red);

		aimEvent.Invoke(aimPoint);
		detectEvent.Invoke(ray);

		movementController.RotateAt(rotationDirection);
	}
	private void UpdatePlayerAttack()
	{
		if (isFire)
		{
			gunController.OnTriggerHold();
		}
		else
		{
			gunController.OnTriggerRelease();
		}
	}
	private void UpdateControlScheme()
	{
		currentControlScheme = playerInput.currentControlScheme;
	}
}

[System.Serializable]
public class AimEvent : UnityEvent<Vector3>
{
}

[System.Serializable]
public class DetectEvent : UnityEvent<Ray>
{
}