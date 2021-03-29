using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
	{
		[Header("Movement")]
		public float movementSpeed = 5f;
		public float acceleration = 12f;
		public AccelerationType accelerationType;
		[Header("Rotation")]
		public bool rotatetWithMovement = true;
		public float rotationSpeed = 12f;
		public RotationType rotationType;

		private Rigidbody rigidbodyComponent;

		private Vector3 movementDirection;
		private Vector3 rotationDirection;
		private Vector3 targetDirection;
		private Quaternion rotation;

		public void SetDirection(Vector3 direction)
		{
			movementDirection = direction;
		}
		public void RotateAt(Vector3 direction)
		{
			rotationDirection = direction;
		}

		public void LookAt(Vector3 point)
		{
			Vector3 heightCorrectedPoint = new Vector3(point.x, transform.position.y, point.z);
			rotationDirection = heightCorrectedPoint - transform.position;
		}

		private void Awake()
		{
			rotation = transform.rotation;
		}
		private void Start()
		{
			rigidbodyComponent = GetComponent<Rigidbody>();
		}
		private void FixedUpdate()
		{
			targetDirection = Accelerate(targetDirection, movementDirection);
			if (rotatetWithMovement && rotationDirection == Vector3.zero)
			{
				if (rotationType == RotationType.MotionDependment)
				{
					RotateTowards(targetDirection);
				}
				else
				{
					RotateTowards(movementDirection);
				}
			}
			RotateTowards(rotationDirection);

			rigidbodyComponent.MoveRotation(rotation);
			rigidbodyComponent.MovePosition(rigidbodyComponent.position + targetDirection * movementSpeed * Time.deltaTime);
			
			DrawDebugLines();
		}

		private Vector3 Accelerate(Vector3 direction, Vector3 targetDirection)
		{
			if (accelerationType == AccelerationType.Interpolation)
			{
				return Vector3.Lerp(direction, targetDirection, acceleration * Time.fixedDeltaTime);
			}
			else
			{
				return Vector3.MoveTowards(direction, targetDirection, acceleration * Time.deltaTime);
			}
		}
		private void RotateTowards(Vector3 direction)
		{
			if (direction.sqrMagnitude > 0.0025f)
			{
				Quaternion targetRotation = Quaternion.LookRotation(direction);
				if (rotationSpeed > 0)
				{
					rotation = Quaternion.Slerp(rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
				}
				else
				{
					rotation = targetRotation;
				}
			}
		}
		private void DrawDebugLines()
		{
			Debug.DrawRay(rigidbodyComponent.position + Vector3.up, targetDirection * 2, Color.red);
			Debug.DrawRay(rigidbodyComponent.position + Vector3.up, movementDirection * 2, Color.green);
			Debug.DrawRay(rigidbodyComponent.position + Vector3.up, transform.forward * 2, Color.blue);
		}
	}
}