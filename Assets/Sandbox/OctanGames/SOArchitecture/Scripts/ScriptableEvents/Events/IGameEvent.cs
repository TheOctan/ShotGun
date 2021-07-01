using OctanGames.ScriptableEvents.Listeners;

namespace OctanGames.ScriptableEvents.Events
{
    public interface IGameEvent
    {
        void Raise();
        void AddListener(IGameEventListener listener);
        void RemoveListener(IGameEventListener listener);
        void RemoveAll();
    }
}
