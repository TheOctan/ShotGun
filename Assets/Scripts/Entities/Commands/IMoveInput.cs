using UnityEngine;

namespace Assets.Scripts.Entities.Commands
{
	public interface IMoveInput
	{
		Vector3 MoveDirection { get; }
	}
}
