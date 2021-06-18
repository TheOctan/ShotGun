using OctanGames.Listeners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.Events
{
    [CreateAssetMenu(menuName = "Scriptable Objects/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<IGameEventListener> eventListeners = new List<IGameEventListener>();

        public void Raise()
		{
			for (int i = eventListeners.Count - 1; i >= 0; i--)
			{
                eventListeners[i].OnEventRaised();
			}
		}

        public void RegisterListener(IGameEventListener listener)
		{
			if (!eventListeners.Contains(listener))
			{
				eventListeners.Add(listener);
			}
		}
		public void UnregisterListener(IGameEventListener listener)
		{
			if (eventListeners.Contains(listener))
			{
				eventListeners.Remove(listener);
			}
		}
	}
}
