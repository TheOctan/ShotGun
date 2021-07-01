using UnityEngine;

namespace OctanGames.ScriptableEvents.Events.Generic
{
	[CreateAssetMenu(menuName = ScriptableEventsUtility.SCRIPTABLE_EVENT + "Game Object Event", order = 51)]
	public class GameObjectEvent : GameEvent<GameObject>
	{
	}
}
