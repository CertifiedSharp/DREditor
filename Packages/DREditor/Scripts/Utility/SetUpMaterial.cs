//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class SetUpMaterial : ScriptableWizard
    {
        /// <summary>
        /// 
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField]
        protected List<string> materialIDs = new()
        { "_BaseMap", "_ParallaxMap", "_MetallicGlossMap", "_OcclusionMap", "_BumpMap" };

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Material> materials = new();

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<List<Texture>> textureGroups = new();


        /// <summary>
        /// 
        /// </summary>
        [MenuItem("DREditor/Utility/Set Up Selected Materials")]
        public static void CreateWizard() => DisplayWizard<SetUpMaterial>("Set Up Selected Materials", "Set Up");


        protected virtual void OnEnable() => InitializeSelectedObjects();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSelectedObjects()
        {
            foreach (var obj in Selection.objects)
                if (obj is Material material)
                    AddMaterial(material);
                else if (obj is Texture texture)
                    AddTexture(texture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="material"></param>
        protected virtual void AddMaterial(Material material)
        {
            materials.Add(material);

            textureGroups.Add(new List<Texture>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        protected virtual void AddTexture(Texture texture)
        {
            if (textureGroups.Count == 0)
                textureGroups.Add(new List<Texture>());

            textureGroups[^1].Add(texture);
        }


        protected virtual void OnWizardCreate() => PopulateMaterials();

        /// <summary>
        ///
        /// </summary>
        protected virtual void PopulateMaterials()
        {
            for (int i = 0; i < materials.Count; i++)
                PopulateMaterial(materials[i], textureGroups[i]);

            UnityEngine.Debug.Log("REPORT: Finished Process");
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