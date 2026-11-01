//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class CustomLayoutUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="actions"></param>
        public static void MassHorizontal(params Action[] actions)
        {
            foreach(Action action in actions)
                DisplayHorizontal(action);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actions"></param>
        public static void MassVertical(params Action[] actions)
        {
            foreach(Action action in actions)
                DisplayVertical(action);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actions"></param>
        public static void DisplayHorizontal(params Action[] actions) =>
            DisplayScoped(new EditorGUILayout.HorizontalScope(), actions);

        /// <summary>
        ///
        /// </summary>
        /// <param name="actions"></param>
        public static void DisplayVertical(params Action[] actions) =>
            DisplayScoped(new EditorGUILayout.VerticalScope(), actions);

        /// <summary>
        ///
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="actions"></param>
        public static void DisplayScoped(GUI.Scope scope, params Action[] actions)
        {
            using (scope)
                foreach (var action in actions)
                    action();
        }
    }
}