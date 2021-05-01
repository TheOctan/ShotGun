using UnityEngine;

namespace OctanGames
{
	public class LockRotation : MonoBehaviour
	{
		private Quaternion startRotation;

		private void Awake()
		{
			startRotation = transform.rotation;
		}

		private void Update()
		{
			transform.rotation = startRotation;
		}
	}
}