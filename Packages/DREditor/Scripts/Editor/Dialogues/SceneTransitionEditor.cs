//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    public class SceneTransitionEditor
    {
        protected SceneTransition SceneTransition;

        public virtual SceneTransition Display(SceneTransition SceneTransition)
        {
            this.SceneTransition = SceneTransition;

            DisplaySceneTransition();

            return this.SceneTransition;
        }

        public virtual void DisplayAddButton()
        {
            if (SceneTransition != null && !SceneTransition.Enabled && GUILayout.Button("Enter Scene", GUILayout.Width(100)))
                SceneTransition.SetEnabled(true);
        }

        protected virtual void DisplaySceneTransition()
        {
            if (SceneTransition.Enabled)
            {
                using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        GUILayout.Label("Name of next Scene:", GUILayout.Width(130));
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            SceneTransition.Scene = GUILayout.TextField(SceneTransition.Scene);
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            SceneTransition.ToDark = HandyFields.Option(SceneTransition.ToDark, "To Dark: ");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            SceneTransition.ToMenu = HandyFields.Option(SceneTransition.ToMenu, "To Menu: ");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            SceneTransition.OnLoadNoDark = HandyFields.Option(SceneTransition.OnLoadNoDark, "OnLoadNoDark: ", 100);
                        }
                        SceneTransition.AtEnd = HandyFields.UnityField(SceneTransition.AtEnd, 130, 30);
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            SceneTransition.MoveToObjective = GUILayout.TextField(SceneTransition.MoveToObjective);
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            SceneTransition.MoveToNextNaturalObjective =
                                HandyFields.Option(SceneTransition.MoveToNextNaturalObjective, "To Next Natural: ", 100);
                        }
                    }
                    if (GUILayout.Button("Remove", GUILayout.Width(80)))
                    {
                        SceneTransition.Scene = string.Empty;

                        SceneTransition.SetEnabled(false);
                    }
                }
            }
        }
    }
}
#endif