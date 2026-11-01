//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Actors.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public static class CircleUtility
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly float DegreesInCircle = 360f;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawCircle"></param>
        /// <param name="circleColor"></param>
        public static void DrawColoredCircle(Action drawCircle, Color circleColor)
        {
            Color lastColor = Handles.color;

            Handles.color = circleColor;

            drawCircle();

            Handles.color = lastColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="normal"></param>
        /// <param name="radius"></param>
        public static void DrawCircle(Vector3 center, Vector3 normal, float radius)
        {
            if (Event.current.type == EventType.Repaint)
                Handles.DrawWireDisc(center, normal, radius);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="circleAreaHeight"></param>
        /// <returns></returns>
        public static Rect CreateDefaultCircleRect(float circleAreaHeight) =>
            GUILayoutUtility.GetRect(0f, circleAreaHeight, GUILayout.ExpandWidth(true));
    }
}