//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

using UnityEditor;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public interface IDebateEditor
    {
        /// <summary>
        ///
        /// </summary>
        public bool EditPanelMode { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int EditPanelNum { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int FocusPhraseIndex { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="focusPhraseIndex"></param>
        void SetFocusPhrase(int focusPhraseIndex);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool IsFocusingPhrase();

        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        SerializedProperty GetPhraseProperty(object phrase);
    }
}