using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.Controllers
{
	public class MovementController : MonoBehaviour
	{
		public Rigidbody rigidbodyComponent;

		[Header("Movement")]
		public float movementSpeed = 5f;
		public float acceleration = 12f;

		[Header("Rotation")]
		public float turnSpeed = 12f;
		public bool velocityDependent = true;
		public bool rotateWithMovement = true;
		public RotationType rotationType;

		[Space]
		public bool alignToCamera = true;

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
			float tangentSpeed = acceleration * Time.fixedDeltaTime;
			movementDirection = AccelerateDirection(movementDirection, rawMovementDirection, tangentSpeed);
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

		private void RotateTowards(Vector3 direction)
		{
			if (direction.sqrMagnitude > 0.0025f)
			{
				Quaternion targetRotation = Quaternion.LookRotation(direction);

				float turnStep = turnSpeed * Time.fixedDeltaTime;
				if (velocityDependent)
				{
					turnStep *= direction.magnitude;
				}

				Quaternion rotation = AccelerateRotation(rigidbodyComponent.rotation, targetRotation, turnStep);

				rigidbodyComponent.MoveRotation(rotation);
			}
		}
		private Vector3 AccelerateDirection(Vector3 direction, Vector3 targetDirection, float tangentSpeed)
		{
			return tangentSpeed > 0 ? Vector3.Lerp(direction, targetDirection, tangentSpeed) : targetDirection;
		}
		private Quaternion AccelerateRotation(Quaternion rotation, Quaternion targetRotation, float turnSpeed)
		{
			return turnSpeed > 0 ? Quaternion.Slerp(rotation, targetRotation, turnSpeed) : targetRotation;
		}
		private Vector3 AlignToCamera(Vector3 direction)
		{
			Vector3 cameraForward = cameraTransform.forward;
			cameraForward.y = 0;

			return Quaternion.LookRotation(cameraForward) * direction;
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