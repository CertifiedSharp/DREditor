//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class ReactionImporter : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [SelectType]
        [SerializeReference]
        [Tooltip("")]
        [SerializeField] protected PresentImportOptionBase importOption;


        /// <summary>
        ///
        /// </summary>
        protected bool exitEarly = false;


        [MenuItem("DREditor/Import/FTE/Present Reactions")]
        public static void SpawnWizard() => DisplayWizard<ReactionImporter>("Reaction Importer", "Apply");

        protected virtual void OnWizardCreate()
        {
            importOption.Initialize(CloseWindowEarly);

            if (exitEarly)
                return;

            importOption.Invoke();

            if (exitEarly)
                return;

            UnityEngine.Debug.Log("REPORT: Option Invocation Process Completed.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CloseWindowEarly()
        {
            exitEarly = true;

            UnityEngine.Debug.Log("REPORT: Aborted Active Wizard.");

            Close();
        }
    }
}
#endif