//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    [Serializable]
    public abstract class LineEditorBase
    {
        protected int testLine = -1;

        public int TestLine { get => testLine; set => testLine = value; }
        protected int i;

        public virtual void EditLine(int i)
        {
            this.i = i;

            DisplayDialogueLine();
        }

        protected abstract void DisplayDialogueLine();

        public virtual void EditTestLine(int groupCount)
        {
            if (testLine != -1)
            {
                GUILayout.Space(25);

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (testLine == 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("Previous Line", GUILayout.Width(100)))
                        testLine -= 1;

                    if (testLine == 0)
                        GUI.enabled = true;

                    GUILayout.Space(50);

                    if (testLine != groupCount - 1 && GUILayout.Button("Next Line", GUILayout.Width(100)))
                        testLine += 1;
                }
            }
        }

        protected virtual void DisplayLineNumberBar()
        {
            DisplayLineTestFocusBar();

            DisplayLineNumberSplitter();
        }

        protected abstract void DisplayLineNumberSplitter();


        protected virtual void DisplayLineTestFocusBar()
        {
            if (TestLine != -1)
                CustomLayoutUtility.DisplayHorizontal(ExitFocusButton, TestButton, DisplayLineNumber);
            else
                CustomLayoutUtility.DisplayHorizontal(DisplayLineNumber, DisplayTestFocus);
        }

        protected virtual void ExitFocusButton()
        {
            if (TestLine != -1 && GUILayout.Button("Exit", GUILayout.Width(60)))
                TestLine = -1;
        }

        protected virtual void DisplayLineNumber() => HandyFields.Label(i.ToString());

        protected virtual void DisplayTestFocus() => CustomLayoutUtility.DisplayHorizontal(TestButton, FocusButton);

        protected virtual void TestButton()
        {
            if (HandyFields.Button("Test"))
                OnTestButtonPressed();
        }

        protected abstract void OnTestButtonPressed();

        
        protected virtual void FocusButton()
        {
            if (HandyFields.Button("Focus"))
                TestLine = i;
        }

        public abstract void DrawHoverSprite();
    }
}
#endif