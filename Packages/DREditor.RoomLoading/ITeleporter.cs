//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    public interface ITeleporter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="fastTravelIndex"></param>
        /// <returns></returns>
        bool RequestTeleport(string sceneName, int fastTravelIndex);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool CanFastTravel();
    }
}