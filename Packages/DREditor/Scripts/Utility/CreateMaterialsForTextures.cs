//Original Author: KHeartz
//Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    /// Scriptable Wizard for creating Material Assets from the Textures selected in the project window.
    /// </summary>
    public class CreateMaterialsForTextures : ScriptableWizard
    {
        /// <summary>
        /// The shader to use for creating each material.
        /// </summary>
        [Tooltip("The shader to use for creating each material.")]
        [SerializeField] protected Shader shader;


        /// <summary>
        /// The actively used texture to create a material with.
        /// </summary>
        protected Texture activeTexture;


        /// <summary>
        /// Creates Material Assets from the Textures selected in the project window.
        /// </summary>
        [MenuItem("DREditor/Utility/Create Materials For Textures")]
        public static void CreateWizard() => DisplayWizard<CreateMaterialsForTextures>("Create Materials", "Create");


        protected virtual void OnWizardCreate()
        {
            using (new AssetDatabase.AssetEditingScope())
                CreateMaterials();
        }

        /// <summary>
        /// Creates Material Assets from the Textures selected in the project window.
        /// </summary>
        protected virtual void CreateMaterials()
        {
            var textures = Selection.GetFiltered(typeof(Texture), SelectionMode.Assets).Cast<Texture>();

            foreach (var texture in textures)
                AssignCreateMaterial(texture);
        }

        /// <summary>
        /// Assigns <see cref="activeTexture"/> and creates a material asset for in the project.
        /// </summary>
        /// <param name="activeTexture">The texture to set as the actively used texture to create a material with.</param>
        protected virtual void AssignCreateMaterial(Texture activeTexture)
        {
            this.activeTexture = activeTexture;

            CreateMaterialFromActiveTexture();
        }

        /// <summary>
        /// Creates a material from <see cref="activeTexture"/>.
        /// </summary>
        protected virtual void CreateMaterialFromActiveTexture()
        {
            string path = GetMaterialPathFromTexture();

            if (!MaterialAlreadyExists(path))
                CreateMaterialAsset(path);
        }

        /// <summary>
        /// Creates a new path for a material using the <see cref="activeTexture"/>.
        /// </summary>
        /// <returns>The path of <see cref="activeTexture"/> modified with the material extension.</returns>
        protected virtual string GetMaterialPathFromTexture()
        {
            if (activeTexture == null)
                return "";

            string path = AssetDatabase.GetAssetPath(activeTexture);

            path = path[..path.LastIndexOf(".")] + ".mat";

            return path;
        }

        /// <summary>
        /// Checks if the material already exists at the given path.
        /// </summary>
        /// <param name="path">The path where a material asset is located.</param>
        /// <returns>True if the material already exists at the given path.</returns>
        protected virtual bool MaterialAlreadyExists(string path)
        {
            bool result = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) != null;

            if (result)
                Debug.LogWarning("ALERT: Can't create material, it already exists at: " + path);

            return result;
        }

        /// <summary>
        /// Creates a new material and creates it as an asset to the project using the <see cref="activeTexture"/>.
        /// </summary>
        /// <param name="path">The path to save the newly created material at.</param>
        private void CreateMaterialAsset(string path)
        {
            if (activeTexture == null)
                return;

            Material mat = new(shader)
            {
                mainTexture = activeTexture,
                color = Color.white
            };

            AssetDatabase.CreateAsset(mat, path);
        }
    }
}
#endif