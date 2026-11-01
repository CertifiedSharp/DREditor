//Dialogue Event List script Author: SeleniumSoul for DREditor
//Used to create automatically create a list of all Dialogue Event Scripts
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

#if UNITY_EDITOR
using UnityEditor;

namespace DREditor.Dialogues.Events
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public class DialogueEventList : TypeListBase<IDialogueEvent>
    {
        static DialogueEventList()
        {
            if (!SessionState.GetBool("FirstInitDone", false))
            {
                UpdateList();

                SessionState.SetBool("FirstInitDone", true);
            }
        }
    }
}
#endif