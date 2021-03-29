using Assets.Scripts.Controllers;
using Assets.Scripts.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Entities.Commands
{
	public class AnalogAim : MonoBehaviour
	{
		[SerializeField] private PlayerController controller;
		[SerializeField, Min(0)] private float aimRadius = 3f;
		[SerializeField] private bool alignToCamera = true;

		private Transform cameraTransform;
		private Vector3 rotationDirection;

		public void OnRotatePlayer(InputAction.CallbackContext context)
		{
			Vector2 inputRotation = context.ReadValue<Vector2>();
			rotationDirection = new Vector3(inputRotation.x, 0, inputRotation.y);
		}

		private void Start()
		{
			cameraTransform = Camera.main.transform;
		}		

		private void Update()
		{
			Vector3 finalRotation = rotationDirection;
			if (alignToCamera)
			{
				finalRotation.AlignToHorizontalDirection(cameraTransform.forward);
			}

			controller.RotateAt(finalRotation);
		}
	}
}