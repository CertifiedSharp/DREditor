//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Dialogues;
using DREditor.Editor.Utility;
using DREditor.Progressing;
using System;
using UnityEditor;

namespace DREditor.Editor.Toolbox
{
    public class MassDialogueEdit : ScriptableWizard
    {
        /// <summary>
        /// 
        /// </summary>
        protected ProgressionDatabase progression;

        /// <summary>
        /// 
        /// </summary>
        protected Variable variable = new();

        /// <summary>
        /// 
        /// </summary>
		protected EndEvent endEvent = new();


        /// <summary>
        /// 
        /// </summary>
		protected VariableEventEditor<Line> VariableEditor;

        /// <summary>
        /// 
        /// </summary>
		protected readonly CustomCallEndEventEditor CustomCallEditor = new();


        [MenuItem("DREditor/Dialogue/Mass Edit Dialogues")]
        public static void CreateWizard() => DisplayWizard<MassDialogueEdit>("Edit Dialogue", "Apply to Dialogues");

        protected virtual void OnEnable()
        {
            if (!DatabaseUtility.InitializeDatabase(progression, out progression) &&
                EditorUtility.DisplayDialog("Progression was not specified.",
                    "Please set up the progression database to " +
                    "use this functionality!", "OK"))
                Close();
            else
                VariableEditor = new(progression);
        }

        protected override bool DrawWizardGUI()
        {
            bool result = base.DrawWizardGUI();

            try
            {
                DisplayGUI();
            }
            catch (ArgumentException) { } // Skipping stupid argument exception about geting control during repaint.

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayGUI()
        {
            VariableEditor?.DisplayAddButton();

            variable = (Variable)VariableEditor?.Display(variable);

            CustomCallEditor.DisplayAddButton();

            endEvent = CustomCallEditor.Display(endEvent);
        }


        protected virtual void OnWizardCreate()
        {
            foreach (UnityEngine.Object o in Selection.objects)
            {
                if (o is not Dialogue dialogue)
                    continue;

                if (variable.Enabled)
                    dialogue.SetVariable(variable);

                if (endEvent.Enabled)
                    dialogue.SetEndEvent(endEvent);

                EditorUtility.SetDirty(o);
            }

            Close();
        }
    }
}
#endif