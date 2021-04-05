using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
	[RequireComponent(typeof(Rigidbody))]
	public class MovementController : MonoBehaviour
	{
		[Header("Movement")]
		public float movementSpeed = 5f;
		public float acceleration = 12f;
		[Header("Rotation")]
		public float turnSpeed = 12f;
		public RotationType rotationType;
		public bool rotateWithMovement = true;
		[Space]
		public bool alignToCamera = true;

		private Rigidbody rigidbodyComponent;
		private Transform cameraTransform;

		private Vector3 rawMovementDirection;
		private Vector3 movementDirection;
		private Vector3 rotationDirection;

		public void SetDirection(Vector3 direction)
		{
			rawMovementDirection = alignToCamera ? AlignToCamera(direction) : direction;
		}
		public void RotateAt(Vector3 direction)
		{
			rotationDirection = alignToCamera ? AlignToCamera(direction) : direction;
		}
		public void LookAt(Vector3 point)
		{
			Vector3 heightCorrectedPoint = new Vector3(point.x, transform.position.y, point.z);
			rotationDirection = (heightCorrectedPoint - transform.position).normalized;
		}

		private void Awake()
		{
			rigidbodyComponent = GetComponent<Rigidbody>();
		}
		private void Start()
		{
			cameraTransform = Camera.main.transform;
		}
		private void FixedUpdate()
		{
			MovePlayer();
			RotatePlayer();
			DrawDebugLines();
		}

		private void MovePlayer()
		{
			movementDirection = Accelerate(movementDirection, rawMovementDirection);
			Vector3 movement = movementDirection * movementSpeed * Time.fixedDeltaTime;
			rigidbodyComponent.MovePosition(rigidbodyComponent.position + movement);
		}

		private void RotatePlayer()
		{
			if (rotateWithMovement && rotationDirection == Vector3.zero)
			{
				if (rotationType == RotationType.MotionDependment)
				{
					RotateTowards(movementDirection);
				}
				else
				{
					RotateTowards(rawMovementDirection);
				}
			}
			else
			{
				RotateTowards(rotationDirection);
			}
		}

		private Vector3 Accelerate(Vector3 direction, Vector3 targetDirection)
		{
			return Vector3.Lerp(direction, targetDirection, acceleration * Time.fixedDeltaTime);
		}

		private void RotateTowards(Vector3 direction)
		{
			if (direction.sqrMagnitude > 0.0025f)
			{
				Quaternion targetRotation = Quaternion.LookRotation(direction);

				Quaternion rotation = (turnSpeed > 0) ?
					Quaternion.Slerp(rigidbodyComponent.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime) :
					targetRotation;

				rigidbodyComponent.MoveRotation(rotation);
			}
		}

		private Vector3 AlignToCamera(Vector3 direction)
		{
			var cameraForward = cameraTransform.forward;
			var cameraRight = cameraTransform.right;

			cameraForward.y = 0f;
			cameraRight.y = 0f;

			return cameraForward * direction.z + cameraRight * direction.x;
		}

		private void DrawDebugLines()
		{
			Debug.DrawRay(rigidbodyComponent.position + Vector3.up, movementDirection * 1.5f, Color.red);
			Debug.DrawRay(rigidbodyComponent.position + Vector3.up, rawMovementDirection * 1.5f, Color.green);
			Debug.DrawRay(rigidbodyComponent.position + Vector3.up, rotationDirection * 1.5f, Color.yellow);
			Debug.DrawRay(rigidbodyComponent.position + Vector3.up, transform.forward * 1.5f, Color.blue);
		}
	}
}