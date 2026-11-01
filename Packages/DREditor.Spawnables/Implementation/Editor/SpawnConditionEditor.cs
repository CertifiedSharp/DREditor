//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Spawnables.Implementation.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(SpawnCondition))]
    public class SpawnConditionEditor : UnityEditor.Editor
    {
        /// <summary>
        ///
        /// </summary>
        protected VariableConditionEditor conditionEditor;

        /// <summary>
        ///
        /// </summary>
        protected SpawnCondition spawnCondition;

        /// <summary>
        ///
        /// </summary>
        protected VariableCapsule conditionCapsule;

        /// <summary>
        ///
        /// </summary>
        protected DialogueEventEditor dialogueEventEditor;

        /// <summary>
        ///
        /// </summary>
        protected GUIStyle wrapStyle;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => InitializeData();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeData()
        {
            spawnCondition = (SpawnCondition)target;

            conditionCapsule = spawnCondition.InstanceData;

            dialogueEventEditor = new(this, conditionCapsule.DiaEvents, conditionCapsule.showDiaEvents);

            conditionEditor = new();
        }

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            CreateForm();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();

                EditorUtility.SetDirty(spawnCondition);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            InitializeWordWrappingStyle();

            DisplayScriptLabels();

            DisplayBasicFields();

            DisplayConditions();

            DisplayDialogueEvents();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeWordWrappingStyle()
        {
            wrapStyle = new(GUI.skin.label)
            {
                wordWrap = true
            };
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayScriptLabels()
        {
            EditorGUILayout.LabelField("When a scene event invokes, evaluates players progression " +
                "and if the condition passes, triggers the dialogue events.", 
                wrapStyle, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 20));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Notice: Because this is Json serialized, Dialogue Events with scriptable objects won't " +
                "save unless they are turned into their own prefab.",
                wrapStyle, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 20));

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayBasicFields()
        {
            spawnCondition.SetSubID(HandyFields.IntField(new GUIContent("Sub ID:", "The integer ID for this savable object. " +
                "If there is another of this scripts type their ID's should not be the same."), spawnCondition.SubID));

            EditorGUILayout.Space();

            spawnCondition.SetSceneEvent(HandyFields.UnityField(new GUIContent("Scene Event:",
                "The scene event to listen to in order to evaluate the condition. Usually on finished loading room."),
                spawnCondition.sceneEvent));

            EditorGUILayout.Space();

            conditionCapsule.SetIgnoreCondition(HandyFields.BoolField(conditionCapsule.ignoreCondition, 
                new GUIContent("Ignore Condition, Just invoke events: ",
                "Whether the condition should not be evaluated and to instead just execute the dialogue." +
                "\nOriginally added to do Map UI Entry Spawns/Events for starts of FTE's.")));

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayConditions()
        {
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                GUIStyle boldStyle = new(EditorStyles.boldLabel);

                EditorGUILayout.LabelField(new GUIContent("Conditions",
                    "The list of conditions required to be passed to invoke the dialogue events."), boldStyle);

                for (int i = 0; i < conditionCapsule.Conditions.Count; i++)
                {
                    DisplayCondition(i);

                    RemoveConditionButton(i);
                }

                AddConditionButton();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayCondition(int i) => 
            conditionCapsule.Conditions[i] = conditionEditor.Display(conditionCapsule.Conditions[i]);

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddConditionButton()
        {
            HandySplitters.UISplitter();

            HandyFields.AddItemButton(conditionCapsule.Conditions, "Condition", new VariableBase(), GUILayout.Width(130));

            HandySplitters.UISplitter();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="conditionIndex"></param>
        protected virtual void RemoveConditionButton(int conditionIndex)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(270);

                HandyFields.RemoveItemButton(conditionCapsule.Conditions, "Condition", conditionIndex, GUILayout.Width(130));
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDialogueEvents()
        {
            dialogueEventEditor.DisplayInterfaceInstances(serializedObject.FindProperty("InstanceData").FindPropertyRelative("DiaEvents"));

            conditionCapsule.SetShowDialogueEvents(dialogueEventEditor.showDiaEvents);

            conditionCapsule.SetDialogueEvents(dialogueEventEditor.instances);

            HandySplitters.UISplitter();
        }
    }
}
#endif