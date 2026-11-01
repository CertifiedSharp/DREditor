//Original Author: KHeartz
//Trial Camera Animation Database Editor for DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    [CustomEditor(typeof(TrialCameraAnimDatabase))]
    public class TrialCameraAnimDatabaseEditor : UnityEditor.Editor
    {
        private TrialCameraAnimDatabase cdb;

        private void OnEnable() => cdb = target as TrialCameraAnimDatabase;

        public override void OnInspectorGUI()
        {
            CreateForm();
            EditorUtility.SetDirty(cdb);
        }
        private void CreateForm()
        {
            EditHeader();
            

            if (cdb.anims == null)
            {
                return;
            }
            if (cdb.animNames.Count != cdb.anims.Count)
            {
                foreach (AnimationClip clip in cdb.anims)
                {
                    cdb.animNames.Add(clip.name);
                    cdb.animValues.Add(false);
                }
            }
            EditVFXs();
            EditAddButton();
        }
        private void EditHeader()
        {
            HandyFields.Label("Trial Camera Animation Database");
            EditorGUILayout.Space(30);

            using (new EditorGUILayout.HorizontalScope())
                DisplayAnimatorController();

            //if (GUILayout.Button("Refresh Animator Controller"))
            //{
            //    RecreateAnimatorController();
            //}
            EditorGUILayout.Space(30);
        }

        protected virtual void DisplayAnimatorController()
        {
            HandyFields.Label("Trial Camera Animator Controller");

            cdb.SetController(HandyFields.UnityField(cdb.controller, 200, 20));

            if (cdb.controller != null && GUILayout.Button("Import Animation Clips") &&
                ConfirmResetClips())
            {
                cdb.ClearAll();

                foreach (var clip in cdb.controller.animationClips)
                    cdb.Add(clip);
            }
        }

        protected virtual bool ConfirmResetClips() =>
            cdb.anims.Count == 0 ||
            EditorUtility.DisplayDialog("Reset Database", 
                "ALERT: \nThis will reset bindings and can potentially mess up currently assigned camera animations on ALL trial dialogues, " +
                "\nare you sure?", 
                "Confirm", "Cancel");

        private void EditAddButton()
        {
            if (GUILayout.Button("Add New Camera Animation"))
            {
                cdb.Add();
            }
        }
        private void EditVFXs()
        {
            for (int i = 0; i < cdb.anims.Count; i++)
            {
                var camAnim = cdb.anims[i];
                if (camAnim == null && cdb.animNames[i] == "")
                {
                    continue;
                }
                using (new EditorGUILayout.HorizontalScope("Box"))
                {
                    EditAnim(camAnim, i);
                    GUILayout.FlexibleSpace();
                }
            }
        }
        private void EditAnim(AnimationClip camAnim, int idx)
        {
            GUIStyle expr = new GUIStyle();
            EditorGUILayout.LabelField(GUIContent.none, expr, GUILayout.Width(100), GUILayout.Height(25));

            using (new EditorGUILayout.HorizontalScope("Box"))
            {
                var bigLabelStyle = new GUIStyle()
                {
                    fontSize = 25,
                    fontStyle = FontStyle.Bold
                };
                //GUILayout.Label(camAnim.animClip.name, bigLabelStyle);

                //GUILayout.FlexibleSpace();
                

                if (cdb.animValues[idx])
                {
                    cdb.animNames[idx] = EditorGUILayout.TextField(cdb.animNames[idx], GUILayout.Width(170));
                }
                else
                {
                    cdb.anims[idx] = HandyFields.UnityField(camAnim, 170, 20);
                    if(cdb.anims[idx] != null)
                    {
                        cdb.animNames[idx] = cdb.anims[idx].name;
                    }
                }
                cdb.animValues[idx] = EditorGUILayout.Toggle(cdb.animValues[idx]);
                

                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    cdb.anims.Remove(camAnim);
                    cdb.animNames.Remove(cdb.animNames[idx]);
                    cdb.animValues.Remove(cdb.animValues[idx]);
                }
            }
        }
        //private AnimatorController GetAnimatorController()
        //{
        //    var dir = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(cdb));
        //    var controllerPath = System.IO.Path.Combine(dir, "animDatabaseController.controller");
        //    var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        //    if (controller != null)
        //    {
        //        ClearAllStatesAndParameters(controller);
        //        return controller;
        //    }
        //    return AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        //}
        //private void RecreateAnimatorController()
        //{
        //    var controller = GetAnimatorController();
        //    var rootStateMachine = controller.layers[0].stateMachine;
        //    rootStateMachine.AddEntryTransition(rootStateMachine.AddState("New State"));
        //    foreach (var anim in cdb.anims)
        //    {
        //        if (anim == null)
        //        {
        //            continue;
        //        }
        //        var animState = controller.AddMotion(anim);
        //        controller.AddParameter(anim.name, AnimatorControllerParameterType.Trigger);
        //        var transition = rootStateMachine.AddAnyStateTransition(animState);
        //        transition.AddCondition(AnimatorConditionMode.If, 0, anim.name);
        //        transition.duration = 0;
        //    }
        //    RefreshAnimationWindow(controller);
        //}
        //private void ClearAllStatesAndParameters(AnimatorController controller)
        //{
        //    for (int i = controller.parameters.Length - 1; i >= 0; i--)
        //    {
        //        controller.RemoveParameter(i);
        //    }
        //    var rootStateMachine = controller.layers[0].stateMachine;
        //    for (int i = rootStateMachine.entryTransitions.Length - 1; i >= 0; i--)
        //    {
        //        rootStateMachine.RemoveState(rootStateMachine.entryTransitions[i].destinationState);
        //    }
        //    for (int i = rootStateMachine.anyStateTransitions.Length - 1; i >= 0; i--)
        //    {
        //        rootStateMachine.RemoveState(rootStateMachine.anyStateTransitions[i].destinationState);
        //    }
        //}

        //private void RefreshAnimationWindow(AnimatorController controller)
        //{
        //    Selection.activeObject = controller;
        //    EditorWindow.GetWindow<EditorWindow>("Animator").Close();
        //    EditorApplication.ExecuteMenuItem("Window/Animation/Animator");
        //    Selection.activeObject = cdb;
        //}
    }
}

#endif