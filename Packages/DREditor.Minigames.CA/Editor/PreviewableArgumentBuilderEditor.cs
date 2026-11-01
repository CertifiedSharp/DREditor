// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.CA.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(ArgumentBuilder))]
    public class PreviewableArgumentBuilderEditor : ArgumentBuilderEditor<ArgumentBuilder>
    {
        /// <summary>
        ///
        /// </summary>
        protected PreviewRenderUtility _previewScene;

        /// <summary>
        ///
        /// </summary>
        protected Texture _previewTexture;

        /// <summary>
        ///
        /// </summary>
        protected bool showLocks;

        /// <summary>
        ///
        /// </summary>
        protected bool updatePreview = true;


        /// <summary>
        ///
        /// </summary>
        /// <param name="updatePreview"></param>
        protected virtual void SetUpdatePreview(bool updatePreview) => this.updatePreview = updatePreview;

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetUpdatePreviewCallback() => SetUpdatePreview(true);


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            previewCallback = SetUpdatePreviewCallback;
        }

        protected virtual void OnDisable() => _previewScene?.Cleanup();


        /// <inheritdoc/>
        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (Event.current.type == EventType.Repaint && _previewTexture != null)
                GUI.DrawTexture(r, _previewTexture, ScaleMode.ScaleToFit, false);
        }

        /// <inheritdoc/>
        protected override void CreateForm()
        {
            DisplayPreviewAsset();

            base.CreateForm();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPreviewAsset()
        {
            EditorGUI.BeginChangeCheck();

            GameObject newPrefab = EditorGUILayout.ObjectField(
                new GUIContent(
                    "Preview Prefab: ",
                    "When assigned, displays you builders manager through a viewport on this object."),
                ClosingArgumentSettings.Instance.PreviewPrefab,
                typeof(GameObject),
                false) as GameObject;

            if (EditorGUI.EndChangeCheck())
                ClosingArgumentSettings.Instance.PreviewPrefab = newPrefab;
        }

        #region Initialize/Has Preview GUI

        /// <inheritdoc/>
        public override bool HasPreviewGUI()
        {
            try
            {
                if (updatePreview)
                    InitializePreview();
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogException(exception);

                _previewScene?.EndPreview();
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePreview()
        {
            argumentPageDisplayer.Initialize(builder.GetTotalStock());

            InitializePreviewScene();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePreviewScene()
        {
            _previewScene?.Cleanup();

            _previewScene = new PreviewRenderUtility();

            InitializePreviewCamera();

            if (ClosingArgumentSettings.Instance.PreviewPrefab == null)
                return;

            _previewTexture = CreatePreviewTexture();

            SetUpdatePreview(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePreviewCamera()
        {
            var cam = _previewScene.camera;

            cam.transform.position = new Vector3(0, 2, -20);

            cam.transform.LookAt(Vector3.zero);

            cam.fieldOfView = 30f;

            cam.nearClipPlane = 0.3f;

            cam.farClipPlane = 1000f;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Texture CreatePreviewTexture()
        {
            _previewScene.BeginPreview(new Rect(0, 0, 1920, 1080), GUIStyle.none);

            CreatePreviewScene();

            _previewScene.camera.Render();

            return _previewScene.EndPreview();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreatePreviewScene()
        {
            GameObject previewObj = _previewScene.InstantiatePrefabInScene(ClosingArgumentSettings.Instance.PreviewPrefab);

            previewObj.transform.position = Vector3.zero;

            InitializePreview(previewObj);

            InvokeLockPreviews(previewObj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="previewObj"></param>
        protected virtual void InitializePreview(GameObject previewObj)
        {
            if (builder.pages.Count > 0 && previewObj.TryGetComponent(out IArgumentPreviewInitializer init))
                init.InitializePreview(builder, currentPage, argumentPageDisplayer.GetCurrentStockIndex() - 1);
            else
                previewObj.GetComponentInChildren<IArgumentPreviewInitializer>()
                    .InitializePreview(builder, currentPage, argumentPageDisplayer.GetCurrentStockIndex() - 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="previewObj"></param>
        protected virtual void InvokeLockPreviews(GameObject previewObj)
        {
            previewObj.GetComponentsInChildren<MonoBehaviour>()
                .Where(lockable => lockable is ILockPreview)
                .Cast<ILockPreview>()
                .ToList()
                .ForEach(lockable => lockable.PreviewLockState(showLocks));
        }

        #endregion

        #region Preview Settings

        /// <inheritdoc/>
        public override void OnPreviewSettings()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                DisplayPreviewPageButtons();

                argumentPageDisplayer.DisplayPreviewStockButtons(SetUpdatePreviewCallback);

                DisplayLockPreviewToggle();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPreviewPageButtons()
        {
            string previewLabel = builder.pages.Count == 0 ?
                "No Page to Preview" :
                $"Previewing page {currentPage}/{builder.pages.Count}";

            GUILayout.Label(previewLabel);

            GUI.enabled = currentPage > 1;

            PageButtonPreview("<", -1);

            GUI.enabled = currentPage < builder.pages.Count;

            PageButtonPreview(">", 1);

            GUI.enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="addPage"></param>
        protected virtual void PageButtonPreview(string buttonText, int addPage)
        {
            if (GUILayout.Button(buttonText))
            {
                currentPage += addPage;

                previewCallback?.Invoke();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayLockPreviewToggle()
        {
            GUILayout.Label(new GUIContent("Show Locks?", "Displays locked stock visually within the preview."));

            if (showLocks != GUILayout.Toggle(showLocks, ""))
            {
                SetUpdatePreview(true);

                showLocks = !showLocks;
            }
        }

        #endregion
    }
}