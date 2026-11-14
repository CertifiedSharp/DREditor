//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    public class CustomCallEndEventEditor
    {
        protected EndEvent endEvent;

        public virtual EndEvent Display(EndEvent endEvent)
        {
            this.endEvent = endEvent;

            if (endEvent.Enabled)
            {
                using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
                {
                    CustomLayoutUtility.DisplayHorizontal(DisplayCustomCall, DisplayCallBreak);

                    RemoveEndEventButton();
                }
            }

            return this.endEvent;
        }

        public virtual void DisplayAddButton()
        {
            if (endEvent != null && !endEvent.Enabled && GUILayout.Button("Custom Call", GUILayout.Width(100)))
                endEvent.SetEnabled(true);
        }


        protected virtual void DisplayCustomCall() =>
            endEvent.CustomCall = HandyFields.StringField("Custom Call: ", endEvent.CustomCall);

        protected virtual void DisplayCallBreak() =>
            endEvent.CallBreak = HandyFields.Option(endEvent.CallBreak, "Call Break: ", 70);

        protected virtual void RemoveEndEventButton()
        {
            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                endEvent.CustomCall = "";

                endEvent.CallBreak = false;

                endEvent.SetEnabled(false);
            }
        }
    }
}
#endif