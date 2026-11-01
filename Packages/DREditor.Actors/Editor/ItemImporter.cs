//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Actors.Editor
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ItemImporter : IItemImporter
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useMeshSelection = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool addCamsToBodies = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [TextArea(5, 100)]
        public string textArea = "";


        /// <summary>
        /// 
        /// </summary>
        protected GameObject item;

        /// <summary>
        /// 
        /// </summary>
        protected string displayName = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        protected int itemIndex;


        public virtual bool ImporterValid()
        {
            bool result = ItemsCanBeImported();

            if (!result)
                OnImportInvalid();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool ItemsCanBeImported() => !string.IsNullOrEmpty(textArea);

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnImportInvalid() => UnityEngine.Debug.LogWarning("ALERT: Item Field was empty! Aborting process.");


        public virtual void Import()
        {
            ResetValues();

            ImportItemsFromTextArea();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ResetValues() => itemIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ImportItemsFromTextArea()
        {
            using StringReader reader = new(textArea);

            while (reader.ReadLine() is string line && line != null)
                BuildItemFrom(line);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void BuildItemFrom(string line)
        {
            InitializeReadValues(line);

            BuildItemGameObject();

            GenerateItemAttributes();

            SetItemName();

            OnBuiltItemComplete();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        protected virtual void InitializeReadValues(string line)
        {
            displayName = line;

            itemIndex = useMeshSelection ? itemIndex : -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual void BuildItemGameObject() =>
            item = itemIndex != -1 ? BuildQuad() : new GameObject();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual GameObject BuildQuad()
        {
            GameObject gameObject;

            gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);

            var materials = Selection.GetFiltered(typeof(Material), SelectionMode.Assets).Cast<Material>().ToList();

            if (materials.Count > itemIndex)
                gameObject.GetComponent<MeshRenderer>().material = materials[itemIndex];

            return gameObject;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void GenerateItemAttributes()
        {
            item.name = displayName;

            ItemUtility.ApplyItemProperties(item, addCamsToBodies);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetItemName() => item.name = displayName.Replace(" ", "_");

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnBuiltItemComplete() => itemIndex++;
    }
}