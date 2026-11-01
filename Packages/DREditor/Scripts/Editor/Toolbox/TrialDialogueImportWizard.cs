//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using UnityEditor;

namespace DREditor.Editor.Toolbox
{
    public class TrialDialogueImportWizard : GenerateDialogueWizardBase
    {
        [MenuItem("DREditor/Import/Dialogues/Trial Dialogue")]
        public static void CreateWizard() => DisplayWizard<TrialDialogueImportWizard>("Import Dialogues", "Import");

        protected override void Import() => Import<TrialDialogue, TrialLine>();
    }
}
#endif