using Assets.Scripts.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Entities.Commands
{
	public class MouseAim : MonoBehaviour
	{
		[SerializeField] private PlayerController controller;
		[SerializeField, Range(0, 1)] private float aimHeight;

		private Camera viewCamera;
		private Vector2 mousePosition;

		public void OnRotatePlayer(InputAction.CallbackContext context)
		{
			mousePosition = context.ReadValue<Vector2>();			
		}

		private void Start()
		{
			viewCamera = Camera.main;
		}

		private void Update()
		{
			Ray ray = viewCamera.ScreenPointToRay(mousePosition);
			Plane groundPlane = new Plane(Vector3.up, Vector3.up * aimHeight);
			float rayDistance;

			if (groundPlane.Raycast(ray, out rayDistance))
			{
				Vector3 point = ray.GetPoint(rayDistance);
				Debug.DrawLine(ray.origin, point, Color.red);

				controller.LookAt(point);
			}
		}
	}
}