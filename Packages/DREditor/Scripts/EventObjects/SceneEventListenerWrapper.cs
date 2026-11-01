//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public class SceneEventListenerWrapper : ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        private readonly System.Action _action;

        public SceneEventListenerWrapper(System.Action action) => _action = action;

        public virtual void OnEventRaised() => _action();
    }
}