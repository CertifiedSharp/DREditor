//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using DREditor.Editor.Utility;
using DREditor.Characters;

namespace DREditor.Editor.Characters
{
    using Debug = UnityEngine.Debug;

    public static class ImportEditorUtility
    {
        public static void ImportMaterials(Student stu)
        {
            var materials = Selection.GetFiltered(typeof(Material), SelectionMode.Assets).Cast<Material>();

            var mats = materials.ToList();

            Debug.Log("Mats has " + mats.Count);

            try
            {
                foreach (var mat in mats)
                {
                    Expression e = new(mat.name, mat);

                    stu.Expressions.Add(e);

                    stu.Sprites.Add(null);//*
                }
            }
            finally
            {

            }
        }

        public static void ImportSprites(Student stu)
        {
            var sprites = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets).Cast<Texture2D>();

            var spr = sprites.ToList();

            List<Sprite> spri = new();

            Debug.Log("spr has " + spr.Count);

            try
            {
                for (int i = 0; i < spr.Count; i++)
                {
                    spri.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(spr[i])));
                }

                for (int i = 0; i < stu.Sprites.Count; i++)
                {
                    for (int x = 0; x < spri.Count; x++)
                    {
                        if (stu.Sprites[i].Name == spri[x].name)
                        {
                            stu.Sprites[i].SetSprite(spri[x]);

                            Debug.Log("Succsessfully Loaded " + spri[x].name);

                            break;
                        }
                    }
                }
            }
            finally
            {

            }
        }

        public static void MissingSprites(Student stu)
        {
            EditorGUILayout.LabelField("Missing Sprite Material and Texture");
            using (new EditorGUILayout.HorizontalScope())
            {
                stu.MissingMat = HandyFields.UnityField(stu.MissingMat, 150, 20);
                stu.MissingTex = HandyFields.UnityField(stu.MissingTex, 150, 20);
                if (GUILayout.Button("Fill Missing Sprites"))
                {
                    if (stu.MissingTex == null || stu.MissingMat == null)
                    {
                        Debug.LogError("You must fill the material and texture fields to fill sprite list");
                        return;
                    }
                    else
                    {
                        for (var i = 0; i < stu.Expressions.Count; i++)
                        {
                            var expr = stu.Expressions[i];
                            if (expr.Sprite == null)
                            {
                                expr.SetSprite(stu.MissingMat);
                                stu.Sprites[i].SetSprite(stu.MissingTex);
                            }
                        }
                    }
                }
                if (GUILayout.Button("Clear Missing Sprites"))
                {
                    for (var i = 0; i < stu.Expressions.Count; i++)
                    {
                        var expr = stu.Expressions[i];
                        if (expr.Sprite == stu.MissingMat)
                        {
                            expr.SetSprite(null);
                            stu.Sprites[i].SetSprite(null);
                        }
                    }
                }
            }
        }
    }
}
#endif