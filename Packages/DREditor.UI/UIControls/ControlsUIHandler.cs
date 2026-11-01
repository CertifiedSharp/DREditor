//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CallableUIBase) +
        "\n\n")]
    public class ControlsUIHandler : CallableUIBase
    {
        protected virtual void OnEnable() => OnInputUpdated(true);

        protected virtual void OnDisable() => OnInputUpdated(false);
    }
}