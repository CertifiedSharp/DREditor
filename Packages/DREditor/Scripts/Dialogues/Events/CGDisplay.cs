//Show CG Dialogue Event script Author: SeleniumSoul for DREditor. Heavily modified by Sweden#6386 for Eden's Garden
using System;
using UnityEngine;
#if FMOD_UNITY
using FMODUnity;
#endif
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
 //   [Serializable]
	//public struct SCGTuple
	//{
	//	public Sprite CG;
	//	public bool transitionInstant;
	//	[EventRef] public string eventName;
	//	public bool ScreenFadeOut;
	//	public GameObject prefab;
	//	public bool playOnly;
	//}

	/// <summary>
	///
	/// </summary>
	[Serializable]
	public class CGDisplay : IDisplayableDialogueEvent
	{
		//public SCGTuple SCGValue;

		public CGDisplay() { }

		public CGDisplay(Sprite CG) => this.CG = CG;

        public Sprite CG;
        public bool transitionInstant;
#if FMOD_UNITY
		[EventRef] public string eventName;
#else
		public AudioClip eventName;
#endif 
		public bool ScreenFadeOut;
        public GameObject prefab;
        public bool playOnly;

        public void TriggerDialogueEvent()
		{
            if (CG != null || prefab != null)
				DialogueEventSystem.TriggerEvent("ShowCG", this);
			else if(!CG && !prefab)
				DialogueEventSystem.TriggerEvent("HideCG", this);
		}

#if UNITY_EDITOR
		private bool _showHelp = false;
		public void EditorUI(object value = null)
		{
			SerializedProperty c = (SerializedProperty)value;
			c = c.FindPropertyRelative("eventName");//.FindPropertyRelative("SCGValue")

            CG = EditorFields.SpriteField("CG: ", CG, 60, 60);
			transitionInstant = EditorFields.Option(transitionInstant, "Instant Set:", 70);
			ScreenFadeOut = EditorFields.Option(ScreenFadeOut, "Use Full Fade:", 90);
			EditorGUILayout.PropertyField(c, new GUIContent(""), GUILayout.MaxWidth(270));
			prefab = EditorFields.UnityField(prefab, 120, 35);
			playOnly = EditorFields.Option(playOnly, "Play Only:", 70);
		}

		public void ToggleHelpBox()
		{
			_showHelp = !_showHelp;
		}

		public void ShowHelpBox()
        {
			if (_showHelp) EditorGUILayout.HelpBox("Show CG pictures. \n\nNote: You'll need the CG Player.", MessageType.Info, true);
		}
#endif
	}
#if UNITY_EDITOR
	public static class EditorFields
    {
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

        public static Sprite SpriteField(string name, Sprite sprite, int width = 120, int height = 120)
		{
			GUILayout.Label(name, GUILayout.Width(80));
			Sprite result;
			using (new EditorGUILayout.VerticalScope())
			{
				result = UnityField(sprite, width, height);
			}
			return result;
		}

		public static T UnityField<T>(T data, int width = 120, int height = 120) where T : UnityEngine.Object
		{
			T result;
			using (new EditorGUILayout.VerticalScope())
			{
				result = (T)EditorGUILayout.ObjectField(data, typeof(T), false, GUILayout.Width(width), GUILayout.Height(height));
			}
			return result;
		}
		public static bool Option(bool option, string label, float labelWidth = 60)
		{
			using (new EditorGUILayout.HorizontalScope())
            {
				GUILayout.Label(label, GUILayout.Width(labelWidth));
				return EditorGUILayout.Toggle(option, GUILayout.Width(15));
			}
		}
		public static string StringField(string name, string value, string label = null, int textWidth = 200)
		{
			GUI.backgroundColor = Color.white;
			string result;
			using (new EditorGUILayout.HorizontalScope())
			{
				if (label == null)
					GUILayout.Label(name, GUILayout.Width(80));
				else
					GUILayout.Label(label, GUILayout.Width(80));
				result = EditorGUILayout.TextField(value, GUILayout.Width(textWidth));
			}
			GUILayout.FlexibleSpace();
			return result;
		}
		public static int IntField(string name, int value, int textBoxWidth = 200, int labelBoxMulti = 6)
		{
			GUI.backgroundColor = Color.white;
			int result;
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label(name, GUILayout.Width(name.Length * labelBoxMulti));
				result = EditorGUILayout.IntField(value, GUILayout.Width(textBoxWidth));
			}
			GUILayout.FlexibleSpace();
			return result;
		}

		public static float FloatField(string label, float value, int textBoxWidth = 200, int labelBoxMulti = 6)//*Added int field functionality
		{
			GUI.backgroundColor = Color.white;
			float result;
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label(label, GUILayout.Width(label.Length * labelBoxMulti));
				result = EditorGUILayout.FloatField(value, GUILayout.Width(textBoxWidth));
			}
			GUILayout.FlexibleSpace();
			return result;
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
	}
#endif
}