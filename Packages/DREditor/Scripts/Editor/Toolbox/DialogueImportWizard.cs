//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using UnityEditor;

namespace DREditor.Editor.Toolbox
{
    public class DialogueImportWizard : GenerateDialogueWizardBase
    {
        [MenuItem("DREditor/Import/Dialogues/Dialogue")]
        public static void CreateWizard() => DisplayWizard<DialogueImportWizard>("Import Dialogues", "Import");

        protected override void Import() => Import<Dialogue, Line>();
    }
}
#endif