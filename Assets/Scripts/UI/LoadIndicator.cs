using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadIndicator : MonoBehaviour
{
	[SerializeField] private Image crosshair;

	public float rotationSpeed = 150f;
	public bool clockwise;

	private void Update()
	{
		float speed;

		if (clockwise)
		{
			speed = -rotationSpeed;
		}
		else
		{
			speed = rotationSpeed;
		}

		crosshair.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
	}
}
