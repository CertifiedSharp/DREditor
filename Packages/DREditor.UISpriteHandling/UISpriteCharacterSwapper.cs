//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.EventObjects;
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UISpriteHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [CustomDialogueEvent("SwapProtagWindowUISprite", typeof(ChangeUISpriteEventData))]
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(IDRESaveable) + ", " + nameof(IDRELoadable) +
        "\n\n")]
    public class UISpriteCharacterSwapper : DialogueEventHandler, IDRESaveable, IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterUISpriteData defaultWindowCharacterData;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterUISpriteData loadWindowCharacterData;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent windowCharacterFirstName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UISpriteChanger uiSpriteChanger = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string uiWindowSaveKey = "ProtagUISprite";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected CharacterUISpriteData activeWindowCharacterData;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (uiSpriteChanger == null)
                uiSpriteChanger = FindAnyObjectByType<UISpriteChanger>();

            if (defaultWindowCharacterData != null)
                UpdateWindowCharacter(defaultWindowCharacterData);
            else
                UpdateWindowCharacter(loadWindowCharacterData);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            if (o == null)
                UnityEngine.Debug.Log($"REPORT: {nameof(UISpriteCharacterSwapper)}: Event data was null, event will not invoke.");

            if (o is ChangeUISpriteEventData data && data != null)
                UpdateWindowCharacter(data.characterUIData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spriteDataAsset"></param>
        protected virtual void UpdateWindowCharacter(ScriptableObject spriteDataAsset)
        {
            CharacterUISpriteData data = (CharacterUISpriteData)spriteDataAsset;

            activeWindowCharacterData = data;

            if (data == null)
            {
                UnityEngine.Debug.LogWarning("ALERT: The sent/current UISprite Data was null, the UI window sprite positions " +
                    "will not change.");

                return;
            }

            windowCharacterFirstName.SetValue(data.charName);

            uiSpriteChanger.SetUISpritePosition(new Vector2(data.x, data.y));
        }


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveDatas) 
            => saveDatas.Set(uiWindowSaveKey, JsonUtility.ToJson(activeWindowCharacterData, true));

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas)
        {
            JsonUtility.FromJsonOverwrite(saveDatas.Get<string>(uiWindowSaveKey), loadWindowCharacterData);

            if (!string.IsNullOrEmpty(loadWindowCharacterData.charName))
                UpdateWindowCharacter(loadWindowCharacterData);
        }


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("SwapProtagWindowUISprite");
    }
}