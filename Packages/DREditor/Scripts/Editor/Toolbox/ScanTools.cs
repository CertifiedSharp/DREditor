//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    ///
    /// </summary>
    public static class ScanTools
    {
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Scan/For Environment SFX in Wrong Field")]
        public static void FindDialoguesWithWrongEnv()
        {
            var assets = FindAssetsByType<Dialogue>();
            //string dialogueNameList = "";
            int count = 0;
            for (int j = 0; j < assets.Count; j++)
            {
                Dialogue dia = assets[j];

                for (int x = 0; x < dia.Lines.Count; x++)
                {
                    Line currentLine = dia.Lines[x];

                    if (currentLine.EnvSFX != null &&
                        currentLine.EnvSFX != "" &&
                        !currentLine.EnvSFX.Contains("Environment"))
                    {
                        UnityEngine.Debug.Log("Found: " + dia.name + " At line: " + x + " String: " + currentLine.EnvSFX);
                        count++;
                    }



                }
            }
            UnityEngine.Debug.Log("Scan Recognized: " + count + " issues.");
            //EditorGUIUtility.systemCopyBuffer = dialogueNameList;
            //Close();
        }
#endif
        [MenuItem("DREditor/Utility/Scan/For Video Events with Clip")]
        public static void FindDialoguesWithVidEvents()
        {
            var assets = FindAssetsByType<Dialogue>();
            //string dialogueNameList = "";
            int count = 0;
            for (int j = 0; j < assets.Count; j++)
            {
                Dialogue dia = assets[j];

                for (int x = 0; x < dia.Lines.Count; x++)
                {
                    Line currentLine = dia.Lines[x];

                    if (currentLine.DiaEvents.Count > 0 && currentLine.DiaEvents[0] is VideoDisplay d && d != null)
                    {
                        if (d.mainClip != null)
                        {
                            UnityEngine.Debug.Log("Found: " + dia.name + " At line: " + x);
                            count++;
                        }
                    }



                }
            }
            UnityEngine.Debug.Log("Scan Recognized: " + count + " issues.");
            //EditorGUIUtility.systemCopyBuffer = dialogueNameList;
            //Close();
        }


#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Scan/Fix Environment SFX in Wrong Field")]
        public static void ConvertDialoguesWithWrongEnv()
        {
            var assets = FindAssetsByType<Dialogue>();
            //string dialogueNameList = "";
            int count = 0;
            for (int j = 0; j < assets.Count; j++)
            {
                Dialogue dia = assets[j];

                for (int x = 0; x < dia.Lines.Count; x++)
                {
                    Line currentLine = dia.Lines[x];

                    if (currentLine.EnvSFX != null &&
                        currentLine.EnvSFX != "" &&
                        !currentLine.EnvSFX.Contains("Environment"))
                    {
                        string message = "Found: " + dia.name + " At line: " + x + " String: " + currentLine.EnvSFX;
                        UnityEngine.Debug.Log(message);

                        if (EditorUtility.DisplayDialog("Check", message, "Convert", "Don't convert"))
                        {
                            currentLine.SFXs.Add(currentLine.EnvSFX);
                            currentLine.EnvSFX = "";
                            EditorUtility.SetDirty(dia);
                        }

                        count++;
                    }



                }
            }
            UnityEngine.Debug.Log("Scan Recognized: " + count + " issues.");
            //EditorGUIUtility.systemCopyBuffer = dialogueNameList;
            //Close();
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Scan/For Trial Dialogue Missing VO")]
        public static void FindTrialDialoguesWithMissingVO()
        {
            var assets = FindAssetsByType<TrialDialogue>();

            bool check = EditorUtility.DisplayDialog("Check", "Only scan Dialogues with containing TD in the name?", "Yes", "No");

            int count = 0;

            bool inCG = false;

            string copyTrace = "";

            for (int j = 0; j < assets.Count; j++)
            {
                TrialDialogue dia = assets[j];

                if (check && !dia.name.Contains("TD"))
                    continue;

                if (dia.name.Contains("Debug"))
                    continue;

                CheckLineForVO(dia, inCG, count, copyTrace);
            }

            bool copy = EditorUtility.DisplayDialog("Check", "Scan Complete, copy to clipboard?", "Yes", "No");

            if (copy)
                GUIUtility.systemCopyBuffer = copyTrace;

            UnityEngine.Debug.Log("Scan Recognized: " + count + " issues.");
        }

        static void CheckLineForVO(TrialDialogue dia, bool inCG, int count, string copyTrace)
        {
            for (int x = 0; x < dia.Lines.Count; x++)
            {
                TrialLine currentLine = dia.Lines[x];

                if (currentLine != null)
                {
                    var list = currentLine.DiaEvents.OfType<CGDisplay>().ToList();

                    if (list.Count > 0 && list[0] is CGDisplay display && display != null)
                        inCG = display.CG != null;

                    if (!inCG && !currentLine.Text.Contains("$") && !(currentLine.Text == "…") && currentLine.VoiceSFX == ""
                        && currentLine.Speaker != null && currentLine.Speaker.FirstName != "Tutorial"
                        && !(currentLine.Text == "..."))
                    {
                        string trace = dia.name + " At line: " + x;

                        UnityEngine.Debug.Log("Scan Found Missing VO: " + trace);

                        count++;

                        copyTrace += trace + "\n";
                    }
                }
            }
        }
#endif
        [MenuItem("DREditor/Utility/Scan/For Dialogue With Transition + DirectTo")]
        public static void FindDialoguesWithTransition()
        {
            var assets = FindAssetsByType<Dialogue>();

            int count = 0;

            for (int j = 0; j < assets.Count; j++)
            {
                Dialogue dia = assets[j];

                if (dia.SceneTransition.Enabled && !dia.SceneTransition.ToMenu && dia.DirectTo.Enabled)
                {
                    UnityEngine.Debug.Log("Found: " + dia.name);

                    count++;
                }
            }

            UnityEngine.Debug.Log("Scan Recognized: " + count + ".");
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

        #region Dialogue Scriptable Object Scanner
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<UnityEngine.Object> FindSOAssetsByType<T>(bool log = true) where T : UnityEngine.Object
        {
            List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

            if (log)
                UnityEngine.Debug.Log("Scriptable Objects of type: " + typeof(T).ToString() + " has found: " + guids.Length +
                "\n Search string format: " + string.Format("t:{0}", typeof(T)));

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                var asset = AssetDatabase.LoadAssetAtPath(assetPath, AssetDatabase.GetMainAssetTypeAtPath(assetPath));
                if (asset != null)
                {
                    assets.Add(asset);
                }


            }
            return assets;
        }
        public static List<Line> RetrieveGenericLinesFromDialogueObject(UnityEngine.Object o)
        {
            var dia = o as DialogueBase<Line>;
            if (dia != null)
            {
                return dia.Lines;
            }
            else
            {
                object obj = o;
                FieldInfo fieldInfo = obj.GetType().BaseType.GetField("Lines");
                if (fieldInfo == null)
                {
                    UnityEngine.Debug.LogError("Was null");
                }

                object itemValue = fieldInfo.GetValue(obj);

                IList values = (IList)itemValue;

                if (values.Count != 0 && values[0] is Line)
                {
                    return GetLineList<Line>(values);
                }
                else
                {
                    return null;
                }

            }
        }
        public static object RetrieveFieldFromObject(UnityEngine.Object o, string variableName, string typeName, bool useBaseType = true)
        {
            object obj = o;
            FieldInfo fieldInfo;

            if (useBaseType)
                fieldInfo = obj.GetType().BaseType.GetField(variableName);
            else
                fieldInfo = obj.GetType().GetField(variableName);

            if (fieldInfo == null)
            {
                UnityEngine.Debug.Log("Retrieved Field for: " + o.name + " did not seem to have variable named: " + variableName);
                return default;
            }

            object itemValue = fieldInfo.GetValue(obj);

            if (itemValue == null)
            {
                UnityEngine.Debug.Log("Retrieved Value for: " + o.name + "'s Field: " + variableName + " appears to be null");
                return default;
            }

            if (itemValue.GetType().ToString() == typeName)
                return itemValue;
            else
            {
                UnityEngine.Debug.Log("Retrieved Value for: " + o.name + "'s Field: " + variableName + " was not the type specified and was type of: "
                    + itemValue.GetType().ToString());
                return default;
            }
        }
        public static List<T> GetLineList<T>(IList values) where T : class
        {
            List<T> lines = new();
            foreach (var val in values)
                if (val is T l)
                    lines.Add(l);
                else
                    UnityEngine.Debug.LogError("The Value of the Generic List was not of the type specified.");
            return lines;
        }
        #endregion
    }
}
#endif

//[MenuItem("DREditor/Utility/Scan/For Dialogues with leave")] Can be deleted
//public static void ScanForFlashWhite()
//{
//    var assets = FindAssetsByType<Dialogue>();

//    int count = 0;

//    for (int j = 0; j < assets.Count; j++)
//    {
//        Dialogue dia = assets[j];

//        for (int x = 0; x < dia.Lines.Count; x++)
//        {
//            Line currentLine = dia.Lines[x];

//            if (currentLine.Leave)
//            {
//                string message = "Found: " + dia.name + " At line: " + x;
//                UnityEngine.Debug.Log(message);

//                var customEvent = new CustomEvent();
//                customEvent.CEValue.EventName = "ActorFade";
//                currentLine.DiaEvents.Add(customEvent);

//                EditorUtility.SetDirty(dia);

//                count++;
//            }
//        }
//    }

//    UnityEngine.Debug.Log("Scan Recognized: " + count + " dialogues.");
//}


// Inside of DREditor but uses the outer child namespaces for FMOD audio.
// This function should be put on a script in it's own region referencing the child audio.

//[MenuItem("DREditor/Utility/Scan/For Missing Music in the Playlist")]
//public static void FindDialoguesWithMissingPlaylistRefs()
//{
//    var assets = FindAssetsByType<Dialogue>();

//    var jukeBoxes = FindAssetsByType<PlaylistFMOD>();

//    PlaylistFMOD jukeBox = jukeBoxes[0];

//    List<string> missingPlaylistTitles = new List<string>();

//    for (int j = 0; j < assets.Count; j++)
//    {
//        Dialogue dia = assets[j];

//        for (int x = 0; x < dia.Lines.Count; x++)
//        {
//            Line currentLine = dia.Lines[x];

//            if (currentLine.Music != "" && currentLine.Music != "Null" &&
//                jukeBox.Musics.Where(x => x.BGM == currentLine.Music).ToList().Count == 0 &&
//                !missingPlaylistTitles.Contains(currentLine.Music))
//            {
//                missingPlaylistTitles.Add(currentLine.Music);
//                UnityEngine.Debug.Log("Currently Missing Title for Track: " + currentLine.Music + " At line: " + x);
//            }

//        }
//    }

//    UnityEngine.Debug.Log("Finished Scan");
//}