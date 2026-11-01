// Original Author: KHeartz
// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Toolbox;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(Present))]
    public class PresentEditor : InventoryItemEditor, INestedEditorCacheable
    {
        /// <summary>
        ///
        /// </summary>
        protected Present present;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty propID;

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<object, bool> visibilityCaches = new();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<object, int> indexCaches = new();

        /// <summary>
        ///
        /// </summary>
        protected List<object> dataCaches = new();

        /// <summary>
        ///
        /// </summary>
        protected bool showUI = false;

        /// <summary>
        ///
        /// </summary>
        protected InterfaceEditor<IPresentData> presentDataEditor;


        /// <summary>
        ///
        /// </summary>
        private readonly string AttributesFieldName = "attributes";

        /// <summary>
        ///
        /// </summary>
        protected readonly string IDPropertyString = "id";

        /// <summary>
        ///
        /// </summary>
        protected readonly string IDLabel = "ID No. ";


        public Dictionary<object, bool> VisibilityCaches => visibilityCaches;

        public Dictionary<object, int> IndexCaches => indexCaches;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            present = target as Present;

            base.OnEnable();

            presentDataEditor = new(this, present.attributes, true);
        }

        /// <inheritdoc/>
        protected override void InitializeSerializedProperties()
        {
            base.InitializeSerializedProperties();

            propID = serializedObject.FindProperty(IDPropertyString);
        }

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            HandyFields.Label(nameof(Present));

            DisplayItemData();

            EditorUtility.SetDirty(present);

            serializedObject.ApplyModifiedProperties();
        }

        /// <inheritdoc/>
        protected override void DisplayItemData()
        {
            base.DisplayItemData();

            DisplayAttributes();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAttributes()
        {
            var presentDataProperty = serializedObject.FindProperty(AttributesFieldName);

            presentDataEditor.DisplayInterfaceInstances(presentDataProperty, this, presentDataProperty, presentDataEditor);

            showUI = presentDataEditor.showDiaEvents;
        }

        /// <inheritdoc/>
        protected override void DisplayName()
        {
            base.DisplayName();

            DisplayID();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayID() => HandyFields.HardPropertyField(IDLabel, propID);

        /// <inheritdoc/>
        public virtual void AddData(object obj) => dataCaches.Add(obj);

        /// <inheritdoc/>
        public virtual bool TryGetData<T>(out T data)
        {
            data = dataCaches.OfType<T>().FirstOrDefault();

            return data != null;
        }
    }
}

#endif