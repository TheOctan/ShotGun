using Assets.Scripts.Controllers;
using Assets.Scripts.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Entities.Commands
{
	public class InputMove : MonoBehaviour
	{
		[SerializeField] private PlayerController controller;
		[SerializeField] private bool alignToCamera = true;

		private Transform cameraTransform;
		private Vector3 movememtDirection;

		public void OnMovePlayer(InputAction.CallbackContext context)
		{
			Vector2 inputDirection = context.ReadValue<Vector2>();
			movememtDirection = new Vector3(inputDirection.x, 0, inputDirection.y);			
		}

		private void Start()
		{
			cameraTransform = Camera.main.transform;
		}

		private void Update()
		{
			if (alignToCamera)
			{
				movememtDirection.AlignToHorizontalDirection(cameraTransform.forward);
			}

			controller.SetDirection(movememtDirection);
		}
	}
}
