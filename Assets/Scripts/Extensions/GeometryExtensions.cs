using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class GeometryExtensions
	{
		public static void AlignToHorizontalDirection(this Vector3 direction, Vector3 target)
		{
			target.y = 0;
			direction = Quaternion.LookRotation(target) * direction;
		}
	}
}
