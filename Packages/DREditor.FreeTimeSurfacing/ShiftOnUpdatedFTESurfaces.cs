//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using DREditor.UI;
using DREditor.Utility;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FTESurfaceSelectionListener) + ", " + nameof(TrackedIndexListener) +
        "\n\n")]
    public class ShiftOnUpdatedFTESurfaces : FTESurfaceSelectionListener
    {
        /// <inheritdoc/>
        protected override void OnIndexUpdated(int from, int to)
        {
            surfaceService?.GetSurfaceAt(from).InvokeFamily<IClosable>((component) => component?.Close());

            surfaceService?.GetSurfaceAt(to).InvokeFamily<IStartUpable>((component) => component?.StartUp());
        }
    }
}