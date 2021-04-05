using Assets.Scripts.Controllers;
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
	[SerializeField, Min(0)] private float minAimRadius = 2f;
	[SerializeField] private bool useAimDistance = true;
	[SerializeField, Min(0)] private float aimDistance = 2f;

	[Space]
	public AimEvent aimEvent;

	private Camera viewCamera;
	private Vector3 rawInputMovement;
	private Vector3 rotationDirection;

	private string currentControlScheme;
	private const string gamepadControlScheme = "Gamepad";
	private const string keyboardControlScheme = "Keyboard And Mouse";

	public void OnControlsChanged()
	{
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
	public void OnReload(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			gunController.Reload();
		}
	}

	private void Awake()
	{
		currentControlScheme = playerInput.currentControlScheme;
	}
	private void Start()
	{
		viewCamera = Camera.main;
	}
	private void Update()
	{
		UpdatePlayerMovement();
		if (currentControlScheme == keyboardControlScheme)
		{
			UpdateMouseAim();
		}
		else if (currentControlScheme == gamepadControlScheme)
		{
			UpdateAnalogAim();
		}
	}
	private void OnDrawGizmosSelected()
	{
		Handles.color = Color.red;
		Handles.DrawWireDisc(transform.position, Vector3.up, minAimRadius);

		Handles.color = Color.green;
		Handles.DrawWireDisc(transform.position, Vector3.up, minAimRadius + aimDistance);
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

			aimEvent.Invoke(point, ray);
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

		aimEvent.Invoke(aimPoint, ray);
		movementController.RotateAt(rotationDirection);
	}
}

[System.Serializable]
public class AimEvent : UnityEvent<Vector3, Ray>
{

}