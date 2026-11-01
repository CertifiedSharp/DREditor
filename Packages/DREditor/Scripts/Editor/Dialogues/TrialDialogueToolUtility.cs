//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using UnityEngine;
using DREditor.Editor.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public static class TrialDialogueToolUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Trial Command/Set All Selected Random Cam Anims")]
        public static void MassRandomizeCamAnimsDialogue()
        {
            UnityEngine.Debug.Log("TrialDialogueToolUtility Tool called");

            if (EditorUtility.DisplayDialog("Randomize Cam Anims?",
                            "WARNING!: This will overwrite ALL Camera Animations on every line on the selected trial dialogue assets! " +
                            "Are you sure you want to continue?", "Yes", "No"))
            {
                TrialCameraAnimDatabase cameraAnimDatabase = DatabaseUtility.LoadDatabase<TrialCameraAnimDatabase>();

                var o = Selection.objects;

                foreach(var dialogue in o)
                {
                    if (dialogue is TrialDialogue trialDialogue)
                    {
                        for (int i = 0; i < trialDialogue.Lines.Count; i++)
                            trialDialogue.Lines[i].CamAnimIdx = Random.Range(0, cameraAnimDatabase.anims.Count - 1);

                        EditorUtility.SetDirty(trialDialogue);
                    }
                }
            }
        }
    }
}