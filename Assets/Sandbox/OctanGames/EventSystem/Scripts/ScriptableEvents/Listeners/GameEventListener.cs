using OctanGames.Events;
using UnityEngine;
using UnityEngine.Events;

namespace OctanGames.Listeners
{
	public class GameEventListener : MonoBehaviour, IGameEventListener
	{
		[Tooltip("Event to register with.")]
		[SerializeField] private GameEvent _gameEvent;

		[Tooltip("Response to invoke when Event is raised.")]
		[SerializeField] private UnityEvent _eventResponse;

		public GameEvent GameEvent
		{
			get => _gameEvent;
			set => _gameEvent = value;
		}
		public event UnityAction EventResponce
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

		public void OnEventRaised()
		{
			_eventResponse.Invoke();
		}
	}
}
