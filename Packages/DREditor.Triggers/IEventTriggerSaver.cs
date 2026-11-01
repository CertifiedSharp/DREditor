//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    public interface IEventTriggerSaver
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        object Save();

        /// <summary>
        ///
        /// </summary>
        /// <param name="ob"></param>
        void Load(object ob);
    }
}