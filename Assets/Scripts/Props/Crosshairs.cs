using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.Props
{
	public class Crosshairs : MonoBehaviour
	{
		public LayerMask targetMask;
		public SpriteRenderer dot;
		public Color dotHighlightColor;

		[Header("Rotation")]
		public float rotateSpeed = 40;
		public bool clockwize;

		private Color originalDotColor;

		public void DetectTargets(Ray ray)
		{
			if (Physics.Raycast(ray, 100, targetMask))
			{
				dot.color = dotHighlightColor;
			}
			else
			{
				dot.color = originalDotColor;
			}
		}

		private void Start()
		{
			Cursor.visible = false;
			originalDotColor = dot.color;
		}

		private void Update()
		{
			transform.Rotate(Vector3.forward * (clockwize ? rotateSpeed : -rotateSpeed) * Time.deltaTime);
		}
	}
}