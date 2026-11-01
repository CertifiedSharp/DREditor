//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    [CustomPropertyDrawer(typeof(DialogueComponentCollection<>))]
    public class DialogueComponentCollectionDrawer : PropertyDrawer
    {
        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty property;

        /// <summary>
        ///
        /// </summary>
        protected float y;


        /// <summary>
        ///
        /// </summary>
        protected readonly Color slightColorVariant = new(0f, 0f, 0f, 0.08f);

        /// <summary>
        ///
        /// </summary>
        protected readonly float additionalHeight = EditorGUIUtility.singleLineHeight + Padding;

        /// <summary>
        ///
        /// </summary>
        protected readonly GUIStyle buttonStyle = new(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleLeft
        };


        /// <summary>
        ///
        /// </summary>
        private const float RowHeight = 22f;

        /// <summary>
        ///
        /// </summary>
        private const float Padding = 2f;

        /// <summary>
        ///
        /// </summary>
        private const float ExtraPadding = 5f;

        /// <summary>
        ///
        /// </summary>
        private const float OrderWidth = 45f;

        /// <summary>
        ///
        /// </summary>
        private const string ComponentsFieldName = "components";

        /// <summary>
        ///
        /// </summary>
        private const string OrderFieldName = "orderFieldName";


        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            y = position.y;

            var components = GetComponents(property);

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.LabelField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight),
                label,
                EditorStyles.boldLabel);

            y += additionalHeight;

            if (components == null || components.Count == 0)
                EditorGUI.LabelField(
                    new Rect(position.x + ExtraPadding + ExtraPadding, y, position.width, EditorGUIUtility.singleLineHeight),
                    "No components discovered.",
                    EditorStyles.miniLabel);
            else
                DrawCollection(position, components);

            EditorGUI.EndProperty();
        }

        protected virtual void DrawCollection(Rect position, IList components)
        {
            DrawColumnHeaders(position.x, y, position.width);

            y += additionalHeight;

            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] == null)
                {
                    components.RemoveAt(i);

                    return;
                }

                DrawRow(position.x, y, position.width, components[i], i);

                y += RowHeight;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual IList GetComponents(SerializedProperty property)
        {
            this.property = property;

            object target = property.serializedObject.targetObject;

            FieldInfo field = target
                .GetType()
                .GetField(property.propertyPath, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (field == null)
                return null;

            object collection = field.GetValue(target);

            if (collection == null)
                return null;

            FieldInfo componentsList = collection
                .GetType()
                .GetField(ComponentsFieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return componentsList?.GetValue(collection) as IList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        protected virtual void DrawColumnHeaders(float x, float y, float width)
        {
            EditorGUI.LabelField(
                new Rect(x, y, OrderWidth, EditorGUIUtility.singleLineHeight),
                "Order", EditorStyles.miniLabel
            );

            EditorGUI.LabelField(
                new Rect(x + OrderWidth + ExtraPadding, y, width - OrderWidth - ExtraPadding, EditorGUIUtility.singleLineHeight),
                "Component", EditorStyles.miniLabel
            );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="component"></param>
        /// <param name="index"></param>
        protected virtual void DrawRow(float x, float y, float width, object component, int index)
        {
            if (index % 2 == 0)
                EditorGUI.DrawRect(new Rect(x, y, width, RowHeight - Padding), slightColorVariant);

            if (component is not MonoBehaviour behaviour)
                return;

            if (behaviour == null)
                return;

            SerializedObject so = new(behaviour);

            string orderFieldName = property.FindPropertyRelative(OrderFieldName).stringValue;

            SerializedProperty orderProp = so.FindProperty(orderFieldName);

            so.Update();

            var orderPropRect = new Rect(x, y + Padding, OrderWidth, EditorGUIUtility.singleLineHeight);

            if (orderProp != null)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUI.PropertyField(orderPropRect, orderProp, GUIContent.none);

                if (EditorGUI.EndChangeCheck())
                {
                    so.ApplyModifiedProperties();

                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }
            }
            else
                EditorGUI.LabelField(orderPropRect, "—", EditorStyles.centeredGreyMiniLabel);

            var labelRect = new Rect(
                x + OrderWidth + ExtraPadding,
                y + Padding,
                width - OrderWidth - ExtraPadding,
                EditorGUIUtility.singleLineHeight);

            string rowText = behaviour.gameObject.name + " : " + behaviour.GetType().Name;

            if (GUI.Button(labelRect, rowText, buttonStyle))
                EditorGUIUtility.PingObject(behaviour.gameObject);
        }

        /// <inheritdoc/>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var components = GetComponents(property);

            int count = components?.Count ?? 0;

            float height = additionalHeight;

            height = count == 0 ? height + EditorGUIUtility.singleLineHeight : height + additionalHeight + (count * RowHeight);

            return height;
        }
    }
}
#endif