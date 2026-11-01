//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace DREditor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public class SetUpMaterial : ScriptableWizard
    {
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField]
        protected List<string> materialIDs = new()
        { "_BaseMap", "_ParallaxMap", "_MetallicGlossMap", "_OcclusionMap", "_BumpMap" };

        [Tooltip("")]
        [SerializeField] protected List<Material> materials = new();

        [Tooltip("")]
        [SerializeField] protected List<List<Texture>> textureGroups = new();


        [MenuItem("DREditor/Utility/Set Up Selected Materials")]
        public static void CreateWizard() => DisplayWizard<SetUpMaterial>("Set Up Selected Materials", "Set Up");


        protected virtual void OnEnable() => InitializeSelectedObjects();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSelectedObjects()
        {
            foreach (var obj in Selection.objects)
            {
                if (obj is Material mat)
                {
                    materials.Add(mat);

                    textureGroups.Add(new List<Texture>());
                }

                if (obj is Texture texture)
                {
                    if (textureGroups.Count == 0)
                        textureGroups.Add(new List<Texture>());

                    textureGroups[^1].Add(texture);
                }
            }
        }


        protected virtual void OnWizardCreate() => PopulateMaterials();

        /// <summary>
        ///
        /// </summary>
        protected virtual void PopulateMaterials()
        {
            for (int i = 0; i < materials.Count; i++)
                PopulateMaterial(materials[i], textureGroups[i]);

            UnityEngine.Debug.Log("Finished Process");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="material"></param>
        /// <param name="textures"></param>
        protected virtual void PopulateMaterial(Material material, List<Texture> textures)
        {
            for (int i = 0; i < materialIDs.Count; i++)
                material.SetTexture(materialIDs[i], textures[i]);

            EditorUtility.SetDirty(material);
        }
    }
}
#endif