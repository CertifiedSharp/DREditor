//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using DREditor.Gates;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoCreateGateWizard : GenerateWizardBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GateWizardSettings settings = null;


        /// <summary>
        /// 
        /// </summary>
        protected string sceneName;

        /// <summary>
        /// 
        /// </summary>
        protected GateDatabase gateDatabase;


        /// <summary>
        /// 
        /// </summary>
        protected UnityEditor.Editor cachedEditor;


        protected AutoCreateGateWizard() : base(true) { }

        protected override bool DisplayTargetDirectory => false;

        protected override bool DisplayAssetPrefix => false;


        protected override void OnDisable()
        {
            base.OnDisable();

            if (cachedEditor != null)
                DestroyImmediate(cachedEditor);
        }


        protected override bool DrawWizardGUI()
        {
            bool result = base.DrawWizardGUI();

            UnityEditor.Editor.DrawFoldoutInspector(settings, ref cachedEditor);

            return result;
        }


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Gates/Auto Setup Gate")]
        public static void ShowWizard() => DisplayWizard<AutoCreateGateWizard>("Auto Setup Gate", "Create");

        protected override bool OnEnableWizardValid()
        {
            InitializeImportFields();

            SetAssetPrefix(settings.assetPrefix);

            SetDirectory(settings.targetDirectory);

            return ImportValidOrAutoCreate();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeImportFields()
        {
            sceneName = SceneManager.GetActiveScene().name;

            gateDatabase = DatabaseUtility.LoadDatabase<GateDatabase>();

            settings = GateWizardSettings.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool ImportValidOrAutoCreate()
        {
            bool result = ReportImportValid();

            return (!result || !AutoCreateUsingSettings()) && result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool ReportImportValid()
        {
            bool result = ImportValid();

            if (!result)
                UnityEngine.Debug.LogWarning("ALERT: The wizard cannot be used because you don't have a database " +
                    "or the current scene is an already existing gate!");

            return result;
        }


        protected override bool ImportValid() => CreatingDatabaseSucceded() || GateDoesNotExist();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool CreatingDatabaseSucceded() => gateDatabase == null &&
            DatabaseUtility.TryCreateDatabaseAt(targetDirectory, out gateDatabase);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool GateDoesNotExist() => !gateDatabase.Contains(sceneName);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool AutoCreateUsingSettings()
        {
            bool result = settings.permanentlyUseSettings;

            if (result)
                OnWizardCreate();

            return result;
        }


        protected override void Import()
        {
            try
            {
                using (new AssetDatabase.AssetEditingScope())
                    AutoSetUpGate();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("ALERT: Gate Spawning Failed.");

                UnityEngine.Debug.LogException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void AutoSetUpGate()
        {
            gateDatabase.Areas.Add(CreateGateFromScene());

            EditorUtility.SetDirty(gateDatabase);

            UnityEngine.Debug.Log($"REPORT: Spawning of Gate Representing Scene: {sceneName} Completed.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual Gate CreateGateFromScene()
        {
            string gateName = assetPrefix + sceneName.Replace(settings.scenePrefix, "");

            string gatePath = FolderUtility.CreateIntermediateFolders(targetDirectory) + "/";

            Gate gate = CustomEditorUtility.CreateAssetWithFolders<Gate>(gateName, gatePath);

            gate.ToAreaName = sceneName;

            EditorUtility.SetDirty(gate);

            Selection.activeObject = gate;

            return gate;
        }
    }
}