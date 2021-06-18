
namespace OctanGames.Listeners.Generic
{
	public interface IGameEventListener<T>
	{
		void OnEventRaised(T param);
	}
}
