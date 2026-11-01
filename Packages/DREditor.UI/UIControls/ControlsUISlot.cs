//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Input;
using DREditor.Services;
using DREditor.Utility;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IControlsUIRow) +
        "\n\n")]
    public class ControlsUISlot : MonoBehaviour, IControlsUIRow
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image left = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image center = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image right = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI controlKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI controlName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool prioritizeImages = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> filterGroups = new()
        {
            "Delta/Mouse",
            "Press/",
            "Left Button/Left Click",
            "Right Button/Right Click"
        };


        /// <summary>
        /// 
        /// </summary>
        protected IControlRowData row;


        /// <summary>
        /// 
        /// </summary>
        protected readonly string UnboundName = "Unbound";


        /// <inheritdoc/>
        public virtual void DisplayRow(IControlRowData row)
        {
            InitializeRow(row);

            ApplyRowData();

            DisplayActiveSprites();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        protected virtual void InitializeRow(IControlRowData row) => this.row = row;
        
        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyRowData()
        {
            PopulateKey();

            PopulateDescription();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void PopulateKey()
        {
            if (prioritizeImages)
                PopulateImages();
            else if (GetControlKey() is string keyName)
                PopulateText(controlKey, FilterBindingName(keyName));
            else
                PopulateText(controlKey, UnboundName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetControlKey()
        {
            if (!string.IsNullOrEmpty(row.ControlKey))
                return row.ControlKey;
            else if (row.ActionReference != null &&
                GetKeyName(row.ActionReference) is string keyName &&
                keyName != UnboundName)
                return keyName;
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected virtual string GetKeyName(InputAction action)
        {
            if (!ServiceLocator.HasService(out IInputManager inputManager))
                return UnboundName;

            string currentControlScheme = inputManager.GetCurrentScheme();

            int bindingIndex = action.GetBindingIndex(group: currentControlScheme);

            if (bindingIndex == -1)
                bindingIndex = GetGlobalBindingIndex(action);

            if (bindingIndex == -1)
                return UnboundName;
            else if(action.bindings[bindingIndex].isPartOfComposite &&
                bindingIndex - 1 >= 0)
                return action.bindings[bindingIndex - 1].name;
            else 
                return action.GetBindingDisplayString(bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private int GetGlobalBindingIndex(InputAction action)
        {
            for (int i = 0; i < action.bindings.Count; i++)
                if (action.bindings[i].isComposite)
                    continue;
                else if (string.IsNullOrEmpty(action.bindings[i].groups))
                    return i;

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        protected virtual string FilterBindingName(string keyName)
        {
            foreach(var replaceGroup in filterGroups)
                if (replaceGroup.Split("/") is string[] replacePair && replacePair.Length > 1)
                    keyName = keyName.Replace(replacePair[0], replacePair[1]);

            return keyName;
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void PopulateDescription() => PopulateText(controlName, row.ControlName);


        /// <summary>
        /// 
        /// </summary>
        protected virtual void PopulateImages()
        {
            PopulateImage(left, row.Left);

            PopulateImage(center, row.Center);

            PopulateImage(right, row.Right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageComponent"></param>
        /// <param name="sprite"></param>
        protected virtual void PopulateImage(Image imageComponent, Sprite sprite)
        {
            if (imageComponent != null)
                imageComponent.sprite = sprite;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="textComponent"></param>
        /// <param name="text"></param>
        protected virtual void PopulateText(TextMeshProUGUI textComponent, string text)
        {
            if (textComponent != null)
                textComponent.text = text;
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayActiveSprites() => ImageUtility.SetActiveBySprite(left, center, right);
    }
}