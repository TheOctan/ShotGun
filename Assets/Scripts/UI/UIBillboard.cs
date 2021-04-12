using UnityEngine;

namespace OctanGames.UI
{
	[ExecuteAlways]
	public class UIBillboard : MonoBehaviour
	{
		public bool alignAnchor = true;

		private Transform cameraTransform;

		private void OnEnable()
		{
			cameraTransform = Camera.main.transform;
		}

		private void OnDisable()
		{
			cameraTransform = null;
		}

		private void LateUpdate()
		{
			if (cameraTransform)
			{
				Vector3 lookDirection = transform.position + cameraTransform.forward;
				if (alignAnchor)
				{
					transform.LookAt(lookDirection, cameraTransform.up);
				}
				else
				{
					transform.LookAt(lookDirection);
				}
			}
		}
	}
}