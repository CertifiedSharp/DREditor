//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public static class DialogueEventsEditorUtility
    {
        private static readonly List<string> AllCustomEventNames = GetAllCustomEventNames();

        [MenuItem("DREditor/Utility/Verify/Custom Events", false, 4)]
        public static void Verify()
        {
            UnityEngine.Debug.Log("Verifying Custom Events....");

            AssetDatabase.StartAssetEditing();

            string report = "No issues found verifying Custom Events";

            var assets = FindAssetsByType<Dialogue>();

            try
            {
                for (int i = 0; i < assets.Count; i++)
                    VerifyDialogueCustomEvents(assets[i], ref report);
            }
            catch (Exception exception)
            {
                report = "ALERT: There was an issue verifying the custom events, contact the developer for support.";

                UnityEngine.Debug.LogException(exception);
            }
            finally
            {
                UnityEngine.Debug.LogWarning("VERIFICATION REPORT: \n" + report);

                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();

                AssetDatabase.StopAssetEditing();
            }
        }

        static void VerifyDialogueCustomEvents(Dialogue dia, ref string report)
        {
            for (int i = 0; i < dia.Lines.Count; i++)
            {
                Line currentLine = dia.Lines[i];

                var dialogueEvent = currentLine.DiaEvents.Where(IsCustomEvent);

                foreach (CustomEvent ev in dialogueEvent.Cast<CustomEvent>())
                {
                    bool valid = ev.toggleCustom && ev.index < AllCustomEventNames.Count;

                    if (valid &&
                        AllCustomEventNames[ev.index] != ev.EventName &&
                        AllCustomEventNames.IndexOf(ev.EventName) is int num &&
                        num != -1)
                    {
                        report += AppendLog("Changed Index", dia, i);

                        ev.ReplaceList(AllCustomEventNames);

                        ev.SetIndex(num);

                        EditorUtility.SetDirty(dia);
                    }
                    else if (ev.toggleCustom && ev.index >= AllCustomEventNames.Count)
                        report += AppendLog("Invalid Index", dia, i);
                    else if (valid && AllCustomEventNames[ev.index] != ev.EventName)
                        report += AppendLog("Suspected Renaming of: " + ev.EventName + " " + ev.index, dia, i);
                }
            }
        }

        static bool IsCustomEvent(IDialogueEvent dialogueEvent) => 
            dialogueEvent != null && dialogueEvent.GetType() == typeof(CustomEvent);

        public static string AppendLog(string report, Dialogue dia, int lineNum) =>
            report + " On: " + dia.name + " Line: " + lineNum + "\n";

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

        static List<string> GetAllCustomEventNames()
        {
            List<string> list = new();

            LookForAttribute<CustomDialogueEventAttribute>((attribute) =>
            {
                list.Add(attribute.eventName);

                return false;
            });

            return list;
        }

        static void LookForAttribute<T>(Func<T, bool> check) where T : Attribute
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
                foreach (Type type in assembly.GetTypes())
                    if (type.GetCustomAttributes(typeof(T)) is IEnumerable<T> attributes &&
                        attributes.Count() > 0)
                        foreach (T attribute in attributes)
                            if (attribute is T customAttribute && check(customAttribute))
                                return;
        }
    }
}
#endif