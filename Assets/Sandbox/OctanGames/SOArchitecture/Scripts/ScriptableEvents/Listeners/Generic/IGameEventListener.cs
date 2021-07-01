
namespace OctanGames.ScriptableEvents.Listeners.Generic
{
	public interface IGameEventListener<T>
	{
		void OnEventRaised(T param);
	}
}
