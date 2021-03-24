using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float acceleration = 12f;
	[SerializeField] private AccelerationType accelerationType;
	[SerializeField] private bool alignToCamera = true;

	private InputMaster input;
	private Rigidbody rigidbodyComponent;
	private Transform viewXamera;

	private Vector3 targetDirection;
	private Vector3 moveDirection;

	private void Awake()
	{
		viewXamera = Camera.main.transform;
		rigidbodyComponent = GetComponent<Rigidbody>();
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

	}

	private void Update()
	{
		Vector2 directionInput = input.Player.Move.ReadValue<Vector2>();
		targetDirection = new Vector3(directionInput.x, 0, directionInput.y);
		if (alignToCamera)
		{
			targetDirection = AlignToCamera(targetDirection);
		}
	}

	private void FixedUpdate()
	{
		if (accelerationType == AccelerationType.Towards)
		{
			moveDirection = Vector3.MoveTowards(moveDirection, targetDirection, acceleration * Time.deltaTime);
		}
		else
		{
			moveDirection = Vector3.Lerp(moveDirection, targetDirection, acceleration * Time.fixedDeltaTime);
		}

		rigidbodyComponent.MovePosition(rigidbodyComponent.position + moveDirection * moveSpeed * Time.deltaTime);

		Debug.DrawRay(rigidbodyComponent.position + Vector3.up, targetDirection * 2, Color.green);
		Debug.DrawRay(rigidbodyComponent.position + Vector3.up, moveDirection * 2, Color.red);
	}

	private Vector3 AlignToCamera(Vector3 direction)
	{
		Vector3 cameraForward = viewXamera.forward;
		cameraForward.y = 0;
		return Quaternion.LookRotation(cameraForward) * direction;
	}
}
