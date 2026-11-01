//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
#if FMOD_UNITY
using FMODUnity;
#endif
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    [Serializable]
    public class AudioCaptureGroup : IDialogueCaptureGroup, IValidCaptureGroup
    {
        /// <summary>
        ///
        /// </summary>
        private enum AudioCaptureType
        {
            Voice,
            SFX,
            Music,
            Environment
        }


        [Tooltip("")]
        [SerializeField] AudioCaptureType capture = AudioCaptureType.Voice;


        public virtual bool IsPriority => false;

#if FMOD_UNITY
        public virtual bool GroupValid()
        {
            if (EventManager.Events == null || EventManager.Events.Count == 0)
                EventManager.RefreshBanks();

            return true;
        }
#else
        public virtual bool GroupValid() => true;
#endif

#if FMOD_UNITY
        private readonly Dictionary<AudioCaptureType, Func<Line, Action<string>>> AudioSetGroups = new()
#else
        private readonly Dictionary<AudioCaptureType, Func<Line, Action<AudioClip>>> AudioSetGroups = new()
#endif
        {
            [AudioCaptureType.Voice] = (line) => line.SetVoice,
            [AudioCaptureType.SFX] = (line) => line.SFXs.Add,
            [AudioCaptureType.Music] = (line) => line.SetMusic,
            [AudioCaptureType.Environment] = (line) => line.SetEnv,
        };

        public virtual void Capture<T>(ref T line, string capturedText) where T : Line => 
            AudioSetGroups[capture](line).Invoke(FindAudio(capturedText));


#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        /// <param name="pathSuffix"></param>
        /// <returns></returns>
        public static string FindAudio(string pathSuffix)
        {
            foreach (EditorEventRef editorEvent in EventManager.Events)
                if (!string.IsNullOrEmpty(editorEvent.Path) && 
                    editorEvent.Path.EndsWith(pathSuffix, StringComparison.OrdinalIgnoreCase))
                    return editorEvent.Path;

            EditorUtility.DisplayDialog("Audio not found.",
                $"ALERT: [FMOD Search] No event found ending with: '{pathSuffix}'", "OK");

            return null;
        }
#else
        public static AudioClip FindAudio(string clipName)
        {
            string[] guids = AssetDatabase.FindAssets($"{clipName} t:AudioClip");

            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);

                return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            }

            EditorUtility.DisplayDialog("Audio not found.",
                $"ALERT: AudioClip with name '{clipName}' not found.", "OK");

            return null;
        }
#endif
    }
}