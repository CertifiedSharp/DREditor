//Music Playlist script originally Author: SeleniumSoul for DREditor
// Edited by Sweden for FMOD Usage.

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DREditor.Audio.FMOD
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(PlaylistFMOD))]
    public class PlaylistFMODEditor : UnityEditor.Editor
    {
        /// <summary>
        ///
        /// </summary>
        protected PlaylistFMOD playlist;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty musicLines;


        /// <summary>
        ///
        /// </summary>
        public virtual void OnEnable()
        {
            playlist = (PlaylistFMOD)target;

            musicLines = serializedObject.FindProperty("Musics");
        }

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            CreateForm();

            SetDataDirty();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            if (playlist.Musics.Count != 0)
                CreateMusicForm();
            else if (AddMusicButton())
                return;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateMusicForm()
        {
            EditorGUILayout.BeginVertical("Box");

            CreateFormTitle();

            DisplayAllMusicUI();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateFormTitle()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Soundtrack Jukebox", GetFormTitleGUIStyle());

            EditorGUILayout.EndHorizontal();


            EditorStyles.textArea.wordWrap = true;

            EditorStyles.textField.wordWrap = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual GUIStyle GetFormTitleGUIStyle()
        {
            return new()
            {
                fontSize = 25,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAllMusicUI()
        {
            for (int i = 0; i < musicLines.arraySize; i++)
                CreateMusicInstanceGUI(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void CreateMusicInstanceGUI(int i)
        {
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            DisplayTrackNumberTitle(i);


            EditorGUILayout.BeginVertical();

            DisplayPlaylistData(i);

            if (DisplayInstanceUtilityButtons(i))
                return;

            EditorGUILayout.EndVertical();


            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayTrackNumberTitle(int i)
        {
            EditorGUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField(i.ToString(), GetTrackNumberGUIStyle(), GUILayout.Width(50));

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayPlaylistData(int i)
        {
            // Display Track UI Title
            playlist.Musics[i].Title = EditorGUILayout.TextField("Title", playlist.Musics[i].Title);

            EvaluateAudioField(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void EvaluateAudioField(int i)
        {
            if (i < playlist.Musics.Count)
                if (musicLines.arraySize > i && musicLines.GetArrayElementAtIndex(i) != null)
                    DisplayAudioField(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayAudioField(int i)
        {
            var sfxListProperty = musicLines.GetArrayElementAtIndex(i).FindPropertyRelative("BGM");

            EditorGUIUtility.labelWidth = 1;

            EditorGUILayout.PropertyField(sfxListProperty, new GUIContent(""), GUILayout.MinWidth(270));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool DisplayInstanceUtilityButtons(int i)
        {
            EditorGUILayout.BeginHorizontal();

            bool utilityButtonClicked = 
                RemoveMusicInstanceButton(i) || 
                MoveUpButton(i) || 
                MoveDownButton(i) || 
                InsertMusicInstanceButton(i);

            EditorGUILayout.EndHorizontal();

            return utilityButtonClicked;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool RemoveMusicInstanceButton(int i)
        {
            if (GUILayout.Button("-"))
            {
                playlist.Musics.Remove(playlist.Musics[i]);

                musicLines.serializedObject.Update();

                return true;
            }
            else
                return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool MoveUpButton(int i)
        {
            if (i > 0)
            {
                GUI.enabled = true;

                if (GUILayout.Button("ʌ") && i > 0)
                {
                    (playlist.Musics[i], playlist.Musics[i - 1]) = (playlist.Musics[i - 1], playlist.Musics[i]);

                    musicLines.serializedObject.Update();

                    return true;
                }
            }
            else
            {
                GUI.enabled = false;

                GUILayout.Button("ʌ");
            }

            GUI.enabled = true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool MoveDownButton(int i)
        {
            if (i < playlist.Musics.Count - 1)
            {
                GUI.enabled = true;

                if (GUILayout.Button("v"))
                {
                    (playlist.Musics[i], playlist.Musics[i + 1]) = (playlist.Musics[i + 1], playlist.Musics[i]);

                    musicLines.serializedObject.Update();

                    return true;
                }
            }
            else
            {
                GUI.enabled = false;

                GUILayout.Button("v");
            }

            GUI.enabled = true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual GUIStyle GetTrackNumberGUIStyle()
        {
            return new()
            {
                fontSize = 25,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool InsertMusicInstanceButton(int i)
        {
            if (GUILayout.Button("+"))
            {
                playlist.Musics.Insert(i + 1, new MusicFMOD());

                musicLines.serializedObject.Update();

                return true;
            }
            else
                return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AddMusicButton()
        {
            if (GUILayout.Button("Add Music"))
            {
                playlist.Musics.Add(new MusicFMOD());

                musicLines.serializedObject.Update();

                return true;
            }
            else
                return false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetDataDirty()
        {
            EditorUtility.SetDirty(playlist);

            musicLines.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif