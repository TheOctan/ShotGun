using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Experimental
{
	public class PlayerMovement : MonoBehaviour
	{
		public Rigidbody rigidbodyComponent;

		[Header("Movement Settings")]
		public float movementSpeed = 5f;
		public float turnSpeed = 12f;
		public bool alignToCamera = true;
		public bool rotateWithMovement = true;

		private Transform cameraTransform;

		private Vector3 movementDirection;
		private Vector3 rotationDirection;

		public void SetDirection(Vector3 direction)
		{
			movementDirection = alignToCamera ? AlignToCamera(direction) : direction;
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
		}

		private void MovePlayer()
		{
			Vector3 movement = movementDirection * movementSpeed * Time.fixedDeltaTime;
			rigidbodyComponent.MovePosition(rigidbodyComponent.position + movement);
		}

		private void RotatePlayer()
		{
			if (rotateWithMovement && rotationDirection == Vector3.zero)
			{
				RotateTowards(movementDirection);
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

				Quaternion rotation = turnSpeed > 0 ?
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
	}
}