using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerMover : MonoBehaviour
{
	public float moveSpeed = 5.0f;

	[SerializeField] private Crosshairs crosshairs;

	private PlayerControls input;
	private Camera viewCamera;
	private PlayerController controller;

	private void Awake()
	{
		viewCamera = Camera.main;
		controller = GetComponent<PlayerController>();

		input = new PlayerControls();
	}	

	private void OnEnable()
	{
		input.Enable();
	}

	private void OnDisable()
	{
		input.Disable();
	}

	private void Start()
    {
        
    }

	private void Update()
	{
		Vector2 rawInput = input.Player.Move.ReadValue<Vector2>();
		Move(rawInput);
		AimCrosshair();
	}

	private void Move(Vector2 direction)
	{
		Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
		Vector3 moveVelocity = moveDirection * moveSpeed;
		controller.Move(moveVelocity);
	}

	private void AimCrosshair()
	{
		Vector3 mousePosition = input.Player.Aim.ReadValue<Vector2>();

		Ray ray = viewCamera.ScreenPointToRay(mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.up * 0.3f);
		float rayDistance;

		if (groundPlane.Raycast(ray, out rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			Debug.DrawLine(ray.origin, point, Color.red);

			controller.LookAt(point);
			crosshairs.transform.position = point;
			crosshairs.DetectTargets(ray);

			//if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > 1)
			//{
			//	gunController.Aim(point);
			//}
		}
	}
}
