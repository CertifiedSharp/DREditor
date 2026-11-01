//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using DREditor.Progressing;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ProgressionContextEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly ProgressionDatabase progression;


        protected ProgressionContextEditorBase(ProgressionDatabase progression = null) =>
            DatabaseUtility.InitializeDatabase(progression, out this.progression);
    }
}
#endif