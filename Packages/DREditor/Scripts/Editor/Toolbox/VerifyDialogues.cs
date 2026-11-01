//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Editor.Utility;
using DREditor.EventObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    ///
    /// </summary>
    public class VerifyDialogues : ScriptableWizard
    {
#if !FMOD_UNITY
        [SerializeField] protected AudioClip clip;
        [SerializeField] protected AudioClip replaceClip;
#endif
        string text;
        string replaceText;
        string fieldVariableName;
        string fieldTypeName;

        bool changed = false;

        [MenuItem("DREditor/Utility/Verify/Dialogues")]
        public static void CreateWizard()
        {
            DisplayWizard<VerifyDialogues>("Verify Dialogues", "Verify");
        }
        private void OnWizardCreate()
        {
            BeginVerification<Dialogue, Line>();
        }

        private void OnGUI()
        {
            GUILayout.Label("Notice: Verifying Minigames like NSD is under DREditor/Utility/Verify Section under Verify Minigames");
            GUILayout.Space(10);
            if (GUILayout.Button("Get Dialogues that have \n DirectTo + SceneTransition no end and no tomenu"))
                HasDirectToAndSceneTransition();
            //if (GUILayout.Button("Get Dialogues that have \n what I'm looking for."))
            //    FindIt();

            if (GUILayout.Button("Verify Dialogues"))
                OnWizardCreate();
            if (GUILayout.Button("Verify Trial Dialogues"))
                BeginVerification<TrialDialogue, TrialLine>();

            GUILayout.Space(20);
            if (GUILayout.Button("Get Trial Dialogues that contain \n the following text."))
                FindDialogueWithText<TrialDialogue>();
            if (GUILayout.Button("Get Dialogues that contain \n the following text."))
                FindDialogueWithText<Dialogue>();
            text = EditorGUILayout.TextArea(text);

            GUILayout.Space(20);

            if (GUILayout.Button("Fix Dialogues that contain \n the following music with."))
                FindReplaceMusic();

            replaceText = EditorGUILayout.TextArea(replaceText);

            GUILayout.Label("Experimental Tool(s)");

            GUILayout.Space(10);

            if (GUILayout.Button("Get Dialogues that contain \n Last Actor Event"))
                FindDialoguesWithLastActor();

            if (GUILayout.Button("Get Dialogues that contain \n Video Event"))
                FindDialoguesWithVideo();

            if (GUILayout.Button("Get Dialogues that contain \n Scene Events"))
                FindDialoguesWithSceneEvent();

            GUILayout.Space(10);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Get Dialogues that contains \n the specified type of variable"))
                    FindTypeOfVariable();
                fieldVariableName = HandyFields.StringField("Variable Name: ", fieldVariableName, 200, 100);
                fieldTypeName = HandyFields.StringField("Type Name: ", fieldTypeName);
            }
        }

        #region Verify Dialogues
        public void BeginVerification<T, T2>() where T : DialogueBase<T2> where T2 : Line
        {
            var assets = FindAssetsByType<T>();

            for (int i = 0; i < assets.Count; i++)
                VerifyDialogueExpressions<T, T2>(assets[i]);
        }

        public void VerifyDialogueExpressions<T, T2>(T dia) where T : DialogueBase<T2> where T2 : Line
        {
            changed = false;

            for (int i = 0; dia.Lines != null && i < dia.Lines.Count; i++)
            {
                var currentLine = dia.Lines[i];

                if (currentLine.Speaker == null || currentLine.ExpressionNumber == 0)
                    continue;

                // If sprite moved position on characters expression list
                if (currentLine.Speaker.Expressions[currentLine.ExpressionNumber - 1].Name != currentLine.Expression.Name)
                {
                    if (ChangedByExpression(dia, currentLine, i))
                        continue;

                    ChangedByName(dia, currentLine, i);
                }
            }

            if (changed)
                EditorUtility.SetDirty(dia);
        }

        protected virtual bool ChangedByExpression<T, T2>(T dia, T2 currentLine, int i) where T : DialogueBase<T2> where T2 : Line
        {
            if (currentLine.Speaker.Expressions.Contains(currentLine.Expression))
            {
                currentLine.SetExpressionNum(currentLine.Speaker.Expressions.IndexOf(currentLine.Expression) + 1);

                Debug.Log("By Expression Changed " + dia.name + "'s Line " + i + " " +
                        currentLine.Speaker.FirstName + "'s  expression number for " + currentLine.Expression.Name);

                changed = true;

                return true;
            }

            return false;
        }

        protected virtual void ChangedByName<T, T2>(T dia, T2 currentLine, int i) where T : DialogueBase<T2> where T2 : Line
        {
            foreach (Expression e in currentLine.Speaker.Expressions)
            {
                if (e.Name == currentLine.Expression.Name)
                {
                    currentLine.SetExpressionNum(currentLine.Speaker.Expressions.IndexOf(e) + 1);

                    Debug.Log("By Name Changed " + dia.name + "'s Line " + i + " " +
                        currentLine.Speaker.FirstName + "'s expression number for " + e.Name);

                    changed = true;

                    break;
                }
            }
        }

        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
        #endregion

        #region Search Functions
        void HasDirectToAndSceneTransition()
        {
            var assets = FindAssetsByType<Dialogue>();
            for (int j = 0; j < assets.Count; j++)
            {
                Dialogue dia = assets[j];
                if (dia.DirectTo != null && dia.SceneTransition.Enabled && !dia.SceneTransition.AtEnd && !dia.SceneTransition.ToMenu)
                {
                    Debug.Log("Asset Found Called: " + dia.name);
                }
            }
            Close();
        }

        void FindDialogueWithText<T>() where T : UnityEngine.Object
        {
            var assets = ScanTools.FindSOAssetsByType<T>();
            bool foundOne = false;
            for (int j = 0; j < assets.Count; j++)
            {
                var dia = assets[j];
                List<Line> Lines = ScanTools.RetrieveGenericLinesFromDialogueObject(dia);
                for (int x = 0; x < Lines.Count; x++)
                {
                    Line currentLine = Lines[x];
                    if (currentLine.Text.Contains(text))
                    {
                        foundOne = true;
                        Debug.Log("Asset Found Called: " + dia.name + " on line: " + x);
                    }
                }
            }
            if (!foundOne)
            {
                Debug.Log("Couldn't find a dialogue that fit.");
            }
            Close();
        }

        /// <summary>
        ///
        /// </summary>
        void FindTypeOfVariable()
        {
            var assets = ScanTools.FindSOAssetsByType<DialogueBase<Line>>();
            bool foundOne = false;
            for (int j = 0; j < assets.Count; j++)
            {
                var dia = assets[j];
                object o = ScanTools.RetrieveFieldFromObject(dia, fieldVariableName, fieldTypeName);
                if (o != null)
                {
                    Debug.Log(dia.name + " " + fieldVariableName + " Value is: " + o.ToString());
                }
            }
            if (!foundOne)
            {
                Debug.Log("Couldn't find a dialogue that fit.");
            }
            Close();
        }
        #endregion

        void FindDialoguesWithLastActor()
        {
            var assets = FindAssetsByType<Dialogue>();
            for (int j = 0; j < assets.Count; j++)
            {
                Dialogue dia = assets[j];

                for (int x = 0; x < dia.Lines.Count; x++)
                {
                    Line currentLine = dia.Lines[x];

                    var vid = currentLine.DiaEvents.Where(n => n.GetType() == typeof(LastActorSprite));
                    if (vid.Count() > 0)
                    {
                        LastActorSprite v = (LastActorSprite)vid.ElementAt(0);
                        if (v != null)
                        {
                            CharacterDatabase db = (CharacterDatabase)
                                ScanTools.FindSOAssetsByType<CharacterDatabase>(false)[0];

                            Debug.Log("Asset Found Called: " + dia.name + " on line: " + x + " For: " + db.Characters[v.charNum].FirstName);
                        }
                    }



                }
            }
            Close();
        }

        void FindDialoguesWithVideo()
        {
            var assets = FindAssetsByType<Dialogue>();
            string dialogueNameList = "";
            for (int j = 0; j < assets.Count; j++)
            {
                Dialogue dia = assets[j];

                for (int x = 0; x < dia.Lines.Count; x++)
                {
                    Line currentLine = dia.Lines[x];

                    var vid = currentLine.DiaEvents.Where(n => n.GetType() == typeof(VideoDisplay));
                    if (vid.Count() > 0)
                    {
                        VideoDisplay v = (VideoDisplay)vid.ElementAt(0);
                        if (v != null)
                        {
                            Debug.Log("Asset Found Called: " + dia.name + " on line: " + x);
                            dialogueNameList += dia.name + " ";
                        }
                    }
                }
            }
            EditorGUIUtility.systemCopyBuffer = dialogueNameList;
            Close();
        }

        void FindDialoguesWithSceneEvent()
        {
            var assets = FindAssetsByType<Dialogue>();
            //string dialogueNameList = "";
            for (int j = 0; j < assets.Count; j++)
            {
                Dialogue dia = assets[j];

                for (int x = 0; x < dia.Lines.Count; x++)
                {
                    Line currentLine = dia.Lines[x];

                    var sceneEvents = currentLine.Events;
                    if (sceneEvents.Count > 0)
                    {
                        Debug.Log("Asset Found Called: " + dia.name + " on line: " + x);
                        SceneEvent s = sceneEvents[0];
                        if (s != null)
                        {
                            Debug.Log("Asset Found Called: " + dia.name + " on line: " + x + " with scene event: " +
                                s.name);
                            //dialogueNameList += dia.name + " ";
                        }
                    }
                }
            }

            Debug.Log("Finished Scan");
            Close();
        }



        public void FindReplaceMusic()
        {
            int count = 0;

            ReplaceDialogueMusic<Dialogue, Line>(ref count);

            ReplaceDialogueMusic<TrialDialogue, TrialLine>(ref count);

            Debug.Log("Scan Recognized: " + count + " issues.");
        }

        protected virtual void ReplaceDialogueMusic<T, T2>(ref int count) where T: DialogueBase<T2> where T2 : Line
        {
            var assets = FindAssetsByType<T>();

            for (int j = 0; j < assets.Count; j++)
            {
                T dia = assets[j];

                for (int x = 0; x < dia.Lines.Count; x++)
                {
                    T2 currentLine = dia.Lines[x];
#if FMOD_UNITY
                    if (currentLine.Music == text)
#else
                    if (currentLine.Music == clip)
#endif
                    {
                        string message = "Found: " + dia.name + " At line: " + x;
                        Debug.Log(message);
#if FMOD_UNITY
                        currentLine.Music = replaceText;
#else
                        currentLine.Music = replaceClip;
#endif
                        EditorUtility.SetDirty(dia);

                        count++;
                    }
                }
            }
        }
    }
}
#endif



//void FindIt()
//{
//    var assets = FindAssetsByType<Dialogue>();
//    for (int j = 0; j < assets.Count; j++)
//    {
//        Dialogue dia = assets[j];
//        if (dia.Lines[0].Speaker != null && dia.Lines[0].Speaker.FirstName.Contains("Damon"))
//        {
//            Debug.Log("Asset Found Called: " + dia.name + " on line: " + 0);
//        }
//        for (int x = 0; x < dia.Lines.Count; x++)
//        {
//            Line currentLine = dia.Lines[x];
//            /* Dialogues with Last Actor Event
//            var vid = currentLine.DiaEvents.Where(n => n.GetType() == typeof(LastActorSprite));
//            if (vid.Count() > 0)
//            {
//                LastActorSprite v = (LastActorSprite)vid.ElementAt(0);
//                if (v != null)
//                {
//                    Debug.Log("Asset Found Called: " + dia.name + " on line: " + x);
//                }
//            }
//            */
//            /*
//            var vid = currentLine.DiaEvents.Where(n => n.GetType() == typeof(VideoDisplay));
//            if (vid.Count() > 0)
//            {
//                VideoDisplay v = (VideoDisplay)vid.ElementAt(0);
//                if (v.waitToEnd)
//                {
//                    Debug.Log("Asset Found Called: " + dia.name + " on line: " + x);
//                }
//            }
//            */

//        }
//    }
//    Close();
//}