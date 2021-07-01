using OctanGames.ScriptableEvents.Listeners.Generic;

namespace OctanGames.ScriptableEvents.Events.Generic
{
    public interface IGameEvent<T>
    {
        void Raise(T value);
        void AddListener(IGameEventListener<T> listener);
        void RemoveListener(IGameEventListener<T> listener);
        void RemoveAll();
    }
}
