//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// 
    /// </summary>
    [CustomClassEditor(typeof(SceneTransition))]
    public class SceneTransitionEditor : ClassEditorTypeBase<SceneTransition>
    {
        protected override void OnInspectorGUI()
        {
            DisplaySceneTransition();
        }

        protected virtual void DisplaySceneTransition()
        {
            using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Label("Name of next Scene:", GUILayout.Width(130));
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        target.Scene = GUILayout.TextField(target.Scene);
                    }
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        target.ToDark = HandyFields.Option(target.ToDark, "To Dark: ");
                    }
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        target.ToMenu = HandyFields.Option(target.ToMenu, "To Menu: ");
                    }
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        target.OnLoadNoDark = HandyFields.Option(target.OnLoadNoDark, "OnLoadNoDark: ", 100);
                    }
                    target.AtEnd = HandyFields.UnityField(target.AtEnd, 130, 30);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        target.MoveToObjective = GUILayout.TextField(target.MoveToObjective);
                    }
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        target.MoveToNextNaturalObjective =
                            HandyFields.Option(target.MoveToNextNaturalObjective, "To Next Natural: ", 100);
                    }
                }
            }
        }
    }
}
#endif