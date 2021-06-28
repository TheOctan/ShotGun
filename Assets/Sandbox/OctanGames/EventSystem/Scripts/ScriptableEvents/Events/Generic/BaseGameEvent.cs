using OctanGames.ScriptableEvents.Listeners.Generic;
using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.ScriptableEvents.Events.Generic
{
	public abstract class BaseGameEvent<T> : ScriptableObject
	{
		private readonly List<IGameEventListener<T>> eventListeners = new List<IGameEventListener<T>>();

		public void Raise(T param)
		{
			for (int i = eventListeners.Count - 1; i >= 0; i--)
				eventListeners[i].OnEventRaised(param);
		}

		public void RegisterListener(IGameEventListener<T> listener)
		{
			if (!eventListeners.Contains(listener))
				eventListeners.Add(listener);
		}
		public void UnregisterListener(IGameEventListener<T> listener)
		{
			if (eventListeners.Contains(listener))
				eventListeners.Remove(listener);
		}
	}
}
