//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Items;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(InventoryItem))]
    public class InventoryItemEditor : UnityEditor.Editor 
    {
        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty propName;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty propDescription;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty propImage;

        /// <summary>
        ///
        /// </summary>
        protected readonly string VerticalBoxLayoutStyle = "Box";

        /// <summary>
        ///
        /// </summary>
        protected readonly string NamePropertyString = "displayName";

        /// <summary>
        ///
        /// </summary>
        protected readonly string DescriptionPropertyString = "description";

        /// <summary>
        ///
        /// </summary>
        protected readonly string ImagePropertyString = "image";

        /// <summary>
        ///
        /// </summary>
        protected readonly float DescriptionHeight = 75;

        /// <summary>
        ///
        /// </summary>
        protected readonly float DescriptionWidth = 310;

        /// <summary>
        ///
        /// </summary>
        protected readonly string NameLabel = "Name: ";

        /// <summary>
        ///
        /// </summary>
        protected readonly string DescriptionLabel = "Description: ";

        /// <summary>
        ///
        /// </summary>
        protected readonly string ImageLabel = "Image: ";


        protected virtual void OnEnable() => InitializeSerializedProperties();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSerializedProperties()
        {
            propName = serializedObject.FindProperty(NamePropertyString);

            propDescription = serializedObject.FindProperty(DescriptionPropertyString);

            propImage = serializedObject.FindProperty(ImagePropertyString);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayItemData()
        {
            using (new EditorGUILayout.VerticalScope(VerticalBoxLayoutStyle))
                DisplayData();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayData()
        {
            DisplayName();

            DisplayDescription();

            DisplayImage();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayName() => HandyFields.HardPropertyField(NameLabel, propName);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDescription()
            => HandyFields.HardPropertyField(DescriptionLabel, propDescription,
                Screen.width - DescriptionWidth, GUILayout.Height(DescriptionHeight));

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayImage() => HandyFields.LabeledPropertyFieldGenericBG<Sprite>(ImageLabel, propImage);
    }
}

#endif