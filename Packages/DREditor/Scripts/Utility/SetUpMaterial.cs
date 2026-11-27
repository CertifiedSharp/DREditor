//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    /// Scriptable wizard to auto apply all textures to a materials shader en masse.
    /// </summary>
    public class SetUpMaterial : ScriptableWizard
    {
        /// <summary>
        /// List of material ID names for the default unity URP shader.
        /// </summary>
        [Header("Scriptable wizard to auto apply all textures to a materials shader en masse.")]
        [Space]
        [Tooltip("List of material ID names for the default unity URP shader.")]
        [SerializeField]
        protected List<string> materialIDs = new()
        { "_BaseMap", "_ParallaxMap", "_MetallicGlossMap", "_OcclusionMap", "_BumpMap" };

        /// <summary>
        /// The list of materials to have their textures populated.
        /// </summary>
        [Tooltip("The list of materials to have their textures populated.")]
        [SerializeField] protected List<Material> materials = new();

        /// <summary>
        /// The List of groups of textures that correspond to a materials textures to populate it's shader's texture fields.
        /// </summary>
        [Tooltip("The List of groups of textures that correspond to a materials textures to populate it's shader's texture fields.")]
        [SerializeField] protected List<List<Texture>> textureGroups = new();


        /// <summary>
        /// Creates a Scriptable wizard to auto apply all textures to a materials shader en masse.
        /// </summary>
        [MenuItem("DREditor/Utility/Set Up Selected Materials")]
        public static void CreateWizard() => DisplayWizard<SetUpMaterial>("Set Up Selected Materials", "Set Up");


        protected virtual void OnEnable() => InitializeSelectedObjects();

        /// <summary>
        /// Populates the <see cref="materials"/> and <see cref="textureGroups"/> fields using the objects currently selected in the project window.
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
        /// Adds a material to the current list of materials and adds a new corresponding texture group with it.
        /// </summary>
        /// <param name="material">The new material to add to the material list.</param>
        protected virtual void AddMaterial(Material material)
        {
            materials.Add(material);

            textureGroups.Add(new List<Texture>());
        }

        /// <summary>
        /// Adds a texture to the latest group of textures.
        /// </summary>
        /// <param name="texture">The texture to add to the latest group of textures.</param>
        protected virtual void AddTexture(Texture texture)
        {
            if (textureGroups.Count == 0)
                textureGroups.Add(new List<Texture>());

            textureGroups[^1].Add(texture);
        }


        protected virtual void OnWizardCreate() => PopulateMaterials();

        /// <summary>
        /// For each material, sets each texture of the materials shader using the corresponding indexes material ID's and list of textures.
        /// </summary>
        protected virtual void PopulateMaterials()
        {
            for (int i = 0; i < materials.Count; i++)
                PopulateMaterial(materials[i], textureGroups[i]);

            UnityEngine.Debug.Log("REPORT: Finished Process");
        }

        /// <summary>
        /// Set each texture of the materials shader using the corresponding indexes material ID's and list of textures.
        /// </summary>
        /// <param name="material">The material to apply textures to.</param>
        /// <param name="textures">The different textures to apply to the materials shader.</param>
        protected virtual void PopulateMaterial(Material material, List<Texture> textures)
        {
            for (int i = 0; i < materialIDs.Count; i++)
                material.SetTexture(materialIDs[i], textures[i]);

            EditorUtility.SetDirty(material);
        }
    }
}
#endif