using OctanGames.ScriptableEvents.Events;
using UnityEngine;
using UnityEngine.Events;

namespace OctanGames.ScriptableEvents.Listeners
{
	[AddComponentMenu("Event Listener/Event Listener")]
	public class EventListener : MonoBehaviour, IGameEventListener
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
			_gameEvent.AddListener(this);
		}
		private void OnDisable()
		{
			_gameEvent.RemoveListener(this);
		}

		public void OnEventRaised()
		{
			if (_eventResponse != null)
				_eventResponse.Invoke();
		}
	}
}
