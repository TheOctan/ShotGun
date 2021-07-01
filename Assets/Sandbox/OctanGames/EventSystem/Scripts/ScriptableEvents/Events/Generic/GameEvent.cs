using OctanGames.ScriptableEvents.Listeners.Generic;
using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.ScriptableEvents.Events.Generic
{
	public abstract class GameEvent<T> : ScriptableObject, IGameEvent<T>
	{
		private readonly List<IGameEventListener<T>> _eventListeners = new List<IGameEventListener<T>>();

		public void Raise(T param)
		{
			for (int i = _eventListeners.Count - 1; i >= 0; i--)
				_eventListeners[i].OnEventRaised(param);
		}

		public void AddListener(IGameEventListener<T> listener)
		{
			if (!_eventListeners.Contains(listener))
				_eventListeners.Add(listener);
		}
		public void RemoveListener(IGameEventListener<T> listener)
		{
			if (_eventListeners.Contains(listener))
				_eventListeners.Remove(listener);
		}

		public void RemoveAll()
		{
			_eventListeners.Clear();
		}
	}
}
