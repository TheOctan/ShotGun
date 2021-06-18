using OctanGames.ScriptableEvents.Events.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OctanGames.ScriptableEvents.Listeners.Generic
{
	public abstract class GameEventListener<T> : MonoBehaviour, IGameEventListener<T>
	{
		[Tooltip("Event to register with.")]
		[SerializeField] private GameEvent<T> _gameEvent;

		[Tooltip("Response to invoke when Event is raised.")]
		[SerializeField] private UnityEvent<T> _eventResponse;

		public GameEvent<T> GameEvent
		{
			get => _gameEvent;
			set => _gameEvent = value;
		}
		public event UnityAction<T> EventResponce
		{
			add => _eventResponse.AddListener(value);
			remove => _eventResponse.RemoveListener(value);
		}

		private void OnEnable()
		{
			_gameEvent.RegisterListener(this);
		}

		private void OnDisable()
		{
			_gameEvent.UnregisterListener(this);
		}

		public void OnEventRaised(T param)
		{
			_eventResponse.Invoke(param);
		}

	}
}
