//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEngine;
using DREditor.Dialogues.Events;
using DREditor.Processables;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Investigations
{
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceAssembly:"DREditor", sourceNamespace: "DREditor.Dialogues.Events")]
    [Serializable]
	public class TruthBulletGet : IDisplayableDialogueEvent
	{
		public bool _showHelp = false;

        public TruthBullet truthBullet;
        public int pieceIndex;

        public void TriggerDialogueEvent()
		{
			DialogueEventSystem.TriggerEvent("TruthBulletGet", this);
		}

#if UNITY_EDITOR
        public void EditorUI(object value = null)
		{
			truthBullet = (TruthBullet)EditorGUILayout
                .ObjectField(
                new GUIContent("Show what bullet?", "Choose which Truth bullet to display."),
                truthBullet,
                typeof(TruthBullet), false);

            if (truthBullet != null && truthBullet.OfType<IBuildableDescription>() is IBuildableDescription collection)
                pieceIndex = IntDrop(pieceIndex, collection.GetFragmentNames());
            else
                pieceIndex = -1;
		}
		public void ToggleHelpBox()
		{
			_showHelp = !_showHelp;
		}

		public void ShowHelpBox()
		{
			if (_showHelp) EditorGUILayout.HelpBox("Adds the truth bullet to the players found truth bullets.", MessageType.Info, true);
		}
		public static int IntDrop(int index, string[] arr, float maxWidth = 130)
		{
            var prependedArray = EditorContainerUtil.PrependedList(arr, "None");
            index = EditorGUILayout.IntPopup(index, prependedArray,
                        EditorContainerUtil.Iota(prependedArray.Length, -1), GUILayout.MaxWidth(maxWidth));
            return index;
        }

#endif
	}
#if UNITY_EDITOR
    /// <summary>
    ///
    /// </summary>
    public static class EditorContainerUtil
    {
        public static int[] Iota(int size, int value = 0)
        {
            int[] values = new int[size];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = value++;
            }
            return values;
        }

        public static T[] PrependedList<T>(T[] list, T firstElement)
        {
            T[] newList = new T[list.Length + 1];
            newList[0] = firstElement;
            for (int i = 0; i < list.Length; i++)
            {
                newList[i + 1] = list[i];
            }
            return newList;
        }
    }
#endif
}
