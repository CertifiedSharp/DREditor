//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FlairStrategyBase : FlairSpawnerBase
    {
        /// <summary>
        ///
        /// </summary>
        protected IFlairEvent eventInstance;


        /// <summary>
        ///
        /// </summary>
        /// <param name="eventInstance"></param>
        /// <returns></returns>
        public virtual bool TryToSpawn(IFlairEvent eventInstance)
        {
            InitializeEvent(eventInstance);

            bool result = ShouldBeUsed();

            if (result)
                Spawn(eventInstance);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventInstance"></param>
        protected virtual void InitializeEvent(IFlairEvent eventInstance) => this.eventInstance = eventInstance;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldBeUsed() => eventInstance.GetFlair().GetPrefab() != null;


        /// <inheritdoc/>
        public override void Spawn(IFlairEvent eventInstance)
        {
            InitializeEvent(eventInstance);

            OnSpawnInvoked();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnSpawnInvoked();
    }
}