using Assets.Scripts;
using Assets.Scripts.Controllers;
using Assets.Scripts.Entities.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour, IMoveInput, IRotationInput
{
	public Vector3 MovementDirection { get; private set; }
	public Vector3 RotationDirection { get; private set; }

	[SerializeField] private bool alignToCamera = true;
	[SerializeField] private PlayerController controller;

	private InputMaster input;
	private Transform viewXamera;


	private void Awake()
	{
		input = new InputMaster();
	}

	private void OnEnable()
	{
		input.Player.Enable();
	}

	private void OnDisable()
	{
		input.Player.Disable();
	}

	private void Start()
	{
		viewXamera = Camera.main.transform;
	}

	private void Update()
	{
		Vector2 inputDirection = input.Player.Move.ReadValue<Vector2>();
		Vector2 inputRotation = input.Player.AnalogAim.ReadValue<Vector2>();

		MovementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
		RotationDirection = new Vector3(inputRotation.x, 0, inputRotation.y);

		if (alignToCamera)
		{
			MovementDirection = AlignToCamera(MovementDirection);
			RotationDirection = AlignToCamera(RotationDirection);
		}

		controller.SetDirection(MovementDirection);
		controller.LookAt(RotationDirection);
	}

	private Vector3 AlignToCamera(Vector3 direction)
	{
		Vector3 cameraForward = viewXamera.forward;
		cameraForward.y = 0;
		return Quaternion.LookRotation(cameraForward) * direction;
	}
}
