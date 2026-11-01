//Author: KHeartz
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Utility
{
    public class CreateMaterialsForTextures : ScriptableWizard
    {
        public Shader shader;

        [MenuItem("DREditor/Utility/Create Materials For Textures")]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<CreateMaterialsForTextures>("Create Materials", "Create");
        }

        void OnWizardCreate()
        {
            try
            {
                AssetDatabase.StartAssetEditing();
                var textures = Selection.GetFiltered(typeof(Texture), SelectionMode.Assets).Cast<Texture>();
                foreach (var tex in textures)
                {
                    string path = AssetDatabase.GetAssetPath(tex);
                    path = path[..path.LastIndexOf(".")] + ".mat";
                    if (AssetDatabase.LoadAssetAtPath(path, typeof(Material)) != null)
                    {
                        Debug.LogWarning("Can't create material, it already exists: " + path);
                        continue;
                    }
                    var mat = new Material(shader);
                    mat.mainTexture = tex;
                    mat.color = Color.white;
                    AssetDatabase.CreateAsset(mat, path);
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif