//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using DREditor.UI;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FTESurfaceSelectionListener : TrackedIndexListener
    {
        /// <summary>
        ///
        /// </summary>
        protected IFTESurfaceManager surfaceService;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override bool InitializedSelector() => ServiceValid() && ServiceHasSelectorType();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ServiceValid()
        {
            bool result = ServiceLocator.HasService(out surfaceService);

            if (!result)
                UnityEngine.Debug.LogError($"ALERT: A {nameof(IFTESurfaceManager)} Service is not available! " +
                    $"REPORT: Disabling detection for when a surface is selected!");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ServiceHasSelectorType() => surfaceService.TryGetSelectionHandler(out selector);
    }
}