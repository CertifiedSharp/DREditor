//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEditor;
using UnityEngine;

namespace DREditor.Actors.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class ItemWizard : ScriptableWizard
    {
        /// <summary>
        /// 
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SelectType]
        [SerializeReference] protected IItemImporter importer = new ItemImporter();


        [MenuItem("DREditor/Import/Items")]
        public static void CreateWizard() => DisplayWizard<ItemWizard>("Item Wizard", "Import");

        protected virtual void OnWizardCreate()
        {
            if (ImportValid())
                Import();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool ImportValid() => importer != null && importer.ImporterValid();

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Import() => importer?.Import();
    }
}