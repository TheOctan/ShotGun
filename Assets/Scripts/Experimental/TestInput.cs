using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float acceleration = 12f;
	[SerializeField] private AccelerationType accelerationType;
	[SerializeField] private bool alignToCamera = true;

	[Header("Rotation")]
	[SerializeField] private float rotationSpeed = 12f;
	[SerializeField] private RotationType rotationType;

	private InputMaster input;
	private Rigidbody rigidbodyComponent;
	private Transform viewXamera;

	private Vector3 targetDirection;
	private Vector3 moveDirection;
	private Quaternion moveRotation;

	private void Awake()
	{
		viewXamera = Camera.main.transform;
		rigidbodyComponent = GetComponent<Rigidbody>();
		input = new InputMaster();
		moveRotation = transform.rotation;
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

	}

	private void Update()
	{
		Vector2 inputDirection = input.Player.Move.ReadValue<Vector2>();
		targetDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
		if (alignToCamera)
		{
			targetDirection = AlignToCamera(targetDirection);
		}
	}

	private void FixedUpdate()
	{
		if (accelerationType == AccelerationType.Interpolation)
		{
			moveDirection = Vector3.Lerp(moveDirection, targetDirection, acceleration * Time.fixedDeltaTime);
		}
		else
		{
			moveDirection = Vector3.MoveTowards(moveDirection, targetDirection, acceleration * Time.deltaTime);
		}

		if (rotationType == RotationType.MotionDependment)
		{
			RotateTowards(moveDirection);
		}
		else
		{
			RotateTowards(targetDirection);
		}

		rigidbodyComponent.MoveRotation(moveRotation);
		rigidbodyComponent.MovePosition(rigidbodyComponent.position + moveDirection * moveSpeed * Time.deltaTime);

		Debug.DrawRay(rigidbodyComponent.position + Vector3.up, targetDirection * 2, Color.red);
		Debug.DrawRay(rigidbodyComponent.position + Vector3.up, moveDirection * 2, Color.green);
		Debug.DrawRay(rigidbodyComponent.position + Vector3.up, transform.forward * 2, Color.blue);
	}

	private Vector3 AlignToCamera(Vector3 direction)
	{
		Vector3 cameraForward = viewXamera.forward;
		cameraForward.y = 0;
		return Quaternion.LookRotation(cameraForward) * direction;
	}

	private void RotateTowards(Vector3 target)
	{
		if (target.sqrMagnitude > 0.0025f)
		{
			Quaternion targetRotation = Quaternion.LookRotation(target);
			moveRotation = Quaternion.Slerp(moveRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
		}
	}
}
