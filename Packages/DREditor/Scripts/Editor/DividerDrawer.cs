//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomPropertyDrawer(typeof(DividerAttribute))]
    public class DividerDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            Rect lineRect = new(position.x, position.y + position.height / 2, position.width, 2);

            EditorGUI.DrawRect(lineRect, Color.gray);
        }

        public override float GetHeight() => 10f;
    }
}