//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using System;
using DREditor.Characters;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
    //[Serializable]
    //public struct MCTuple
    //{
    //    public int charNum;
    //    public Vector3 position;
    //    public Vector3 rotation;
    //    public bool all;
    //}
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MoveCharacter : IDisplayableDialogueEvent
    {
        public bool _showHelp = false;
        //public MCTuple data;

        public int charNum;
        public Vector3 position;
        public Vector3 rotation;
        public bool all;

        public void TriggerDialogueEvent()
        {
            DialogueEventSystem.TriggerEvent("MoveCharacter", this);
        }

#if UNITY_EDITOR
        CharacterDatabase database;
        public void EditorUI(object value = null)
        {
            if (!database)
                database = GetDatabase();
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Character to move: ", GUILayout.Width(135));
                charNum = EditorGUILayout.IntPopup(charNum, database.GetNames().ToArray(),
                    database.GetInts().ToArray(), GUILayout.Width(130));
                
                if(GUILayout.Button("Record Character Postion", GUILayout.Width(200)))
                {
                    if (UnityEngine.Object.FindObjectsByType<RectTransform>()
                        .Where(x => x.gameObject.name == database.Characters[charNum].FirstName).First()
                        is RectTransform g && g != null && g.gameObject.layer == 6)
                    {
                        position = g.transform.position;
                        rotation = g.transform.eulerAngles;
                    }
                    else
                        UnityEngine.Debug.LogWarning("That Character: " + database.Characters[charNum].FirstName +
                            " doesn't seem to be in the current scene!");
                }
            }

            position = Vector3Field("Position to go to: ", position);
            rotation = Vector3Field("Rotation to go to: ", rotation);
            using (new EditorGUILayout.HorizontalScope())
            {
                all = EditorFields.Option(all, "Move All: ");
            }
            //moveSprite = EditorFields.Option(moveSprite, "Move SubPiece: ");
        }

        private static CharacterDatabase GetDatabase()
        {
            string[] _databaseguids = AssetDatabase.FindAssets("t:CharacterDatabase");
            string _databasepath = AssetDatabase.GUIDToAssetPath(_databaseguids[0]);
            EditorGUILayout.LabelField($"Loading CharacterDatabase at {_databasepath}.");
            return AssetDatabase.LoadAssetAtPath<CharacterDatabase>(_databasepath);
        }

        public static Vector3 Vector3Field(string label, Vector3 value, int textBoxWidth = 200, int labelBoxMulti = 6)//*
        {
            GUI.backgroundColor = Color.white;
            Vector3 result;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(label, GUILayout.Width(label.Length * labelBoxMulti));
                result = EditorGUILayout.Vector3Field("", value, GUILayout.Width(textBoxWidth));
            }
            GUILayout.FlexibleSpace();
            return result;
        }
        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("Move a character in the scene to a position.", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif
    }
}
