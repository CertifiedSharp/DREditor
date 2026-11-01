//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UI.Options.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class OptionBuilder : MonoBehaviour
    {
#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected Transform optionParent;

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected MenuGroup menuGroup;

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected List<OptionInstance> optionAssets = new();

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected List<OptionType> optionTypes = new();


        /// <summary>
        ///
        /// </summary>
        protected OptionInstance option;

        /// <summary>
        ///
        /// </summary>
        protected GameObject prefab;

        /// <summary>
        ///
        /// </summary>
        protected GameObject spawnedOption;


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Build Options Menu")]
        public void BuildOptionsMenu()
        {
            foreach (var option in optionAssets)
                SpawnOption(option);

            menuGroup.SetFirst(menuGroup.selectables[0].gameObject);

            EditorUtility.SetDirty(menuGroup);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="option"></param>
        protected virtual void SpawnOption(OptionInstance option)
        {
            if (GetPrefabFromName(option.typeName) is GameObject prefab)
            {
                this.option = option;

                this.prefab = prefab;

                spawnedOption = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, optionParent) as GameObject;

                ApplyRename();

                InitializeEvent();

                InitializeSelectables();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected virtual GameObject GetPrefabFromName(string typeName)
        {
            var matches = optionTypes.Where(option => option.typeName == typeName);

            foreach (var match in matches)
                if (match != null)
                    return match.prefab;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyRename()
        {
            string formattedName = Regex.Replace(option.optionAsset.name, @"([a-z0-9])([A-Z])", "$1_$2");

            formattedName = Regex.Replace(formattedName, @"([a-z0-9]) ([A-Z])", "$1_$2");

            spawnedOption.name = formattedName + "_" + prefab.name;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeEvent()
        {
            if (option.optionAsset is IOptionEvent validOption)
            {
                spawnedOption.GetComponent<IOptionEventListener>().Initialize(validOption);

                var childListeners = spawnedOption.GetComponentsInChildren<IOptionEventListener>();

                foreach (var listener in childListeners)
                    listener.Initialize(validOption);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSelectables()
        {
            if (spawnedOption.TryGetComponent(out Selectable selectable) && selectable != null)
                menuGroup.selectables.Add(selectable);
            else
                menuGroup.selectables.Add(spawnedOption.GetComponentsInChildren<Selectable>().First());
        }
#endif
    }
}