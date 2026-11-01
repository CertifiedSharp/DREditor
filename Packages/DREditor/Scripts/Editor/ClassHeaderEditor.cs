//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEditor;

namespace DREditor.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public class ClassHeaderEditor : BehaviourAttributeEditorBase
    {
        public override float Priority => 0;

        public override void Display() => Display<ClassHeaderAttribute>(DrawClassHeader);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawClassHeader(ClassHeaderAttribute attribute)
        {
            EditorStyles.boldLabel.wordWrap = true;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField(attribute.Header, EditorStyles.boldLabel);

            EditorGUILayout.Space();
        }
    }
}