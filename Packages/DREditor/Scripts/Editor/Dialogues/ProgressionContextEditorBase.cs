//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using DREditor.Progressing;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// Base class for a custom class editor for POCO of type: <typeparamref name="T"/>. That uses the progression database.
    /// </summary>
    /// <typeparam name="T">The target class type being edited.</typeparam>
    public abstract class ProgressionContextEditorBase<T> : ClassEditorTypeBase<T>
    {
        /// <summary>
        /// The progression database used for editing the target POCO.
        /// </summary>
        protected readonly ProgressionDatabase progression;


        protected ProgressionContextEditorBase() =>
            DatabaseUtility.InitializeDatabase(progression, out progression);
    }
}
#endif