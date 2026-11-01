//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventComponent) + ", " + nameof(ILineTextDisplay) +
        "\n\n")]
    public class SceneEventOnTextDisplay : SceneEventComponent, ILineTextDisplay
    {
        /// <inheritdoc/>
        public virtual void LineTextDisplay(Line currentLine) => InvokeSceneEvent();
    }
}