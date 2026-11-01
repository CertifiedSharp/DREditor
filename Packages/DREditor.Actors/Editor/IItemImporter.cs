//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Actors.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public interface IItemImporter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ImporterValid();

        /// <summary>
        /// 
        /// </summary>
        public void Import();
    }
}