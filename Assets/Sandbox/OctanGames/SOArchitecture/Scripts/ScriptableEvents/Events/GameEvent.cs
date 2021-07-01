using OctanGames.ScriptableEvents.Listeners;
using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.ScriptableEvents.Events
{
	[CreateAssetMenu(menuName = ScriptableEventsUtility.SCRIPTABLE_EVENT + "Void Event")]
	public class GameEvent : ScriptableObject, IGameEvent
	{
		private readonly List<IGameEventListener> _eventListeners = new List<IGameEventListener>();

		public void Raise()
		{
			for (int i = _eventListeners.Count - 1; i >= 0; i--)
				_eventListeners[i].OnEventRaised();
		}

		public void AddListener(IGameEventListener listener)
		{
			if (!_eventListeners.Contains(listener))
				_eventListeners.Add(listener);
		}
		public void RemoveListener(IGameEventListener listener)
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
