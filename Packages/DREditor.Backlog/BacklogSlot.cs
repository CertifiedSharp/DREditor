// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeotheDev
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.UI;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Backlogging
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class BacklogSlot : MonoBehaviour, IGridIconSlot
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Backlog backlog = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string headmasterFirstName = "Tozu";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] private BacklogMugBind[] Mugshots;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected GameObject voiceIcon = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image overlayImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite overlayOn;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite overlayOff;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI backlogText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image mugshotImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage nameplate = null;


        /// <inheritdoc/>
        public virtual void ClearSlotState() => DisplayEmptyBacklogEntry();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayEmptyBacklogEntry()
        {
            mugshotImage.gameObject.SetActive(false);

            overlayImage.sprite = overlayOff;

            nameplate.gameObject.SetActive(false);

            voiceIcon.SetActive(false);

            backlogText.text = string.Empty;
        }

        
        /// <inheritdoc/>
        public virtual void UpdateSlot(int globalIndex, bool selected)
            => UpdateBacklogSlot(backlog.Lines.ToArray().Reverse().ToArray()[globalIndex], selected);

        /// <summary>
        ///
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="localSelected"></param>
        protected virtual void UpdateBacklogSlot(BacklogLines entry, bool localSelected)
        {
            Character character = characterDatabase.GetCharacter(entry.CharFirstName);

            if (character == null)
                return;

            EvaluateMugshot(character, localSelected, entry.AliasNum);

            nameplate.gameObject.SetActive(true);

            nameplate.texture = entry.AliasNum != -1
                ? characterDatabase.GetCharacter(entry.CharFirstName).Aliases[entry.AliasNum].Nameplate
                : characterDatabase.GetCharacter(entry.CharFirstName).Nameplate;

            overlayImage.sprite = localSelected ? overlayOn : overlayOff;

            backlogText.text = entry.Text;

            backlogText.color = entry.Color;

            voiceIcon.SetActive(!string.IsNullOrEmpty(entry.Voice));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="localSelected"></param>
        /// <param name="entryAliasNum"></param>
        protected virtual void EvaluateMugshot(Character character, bool localSelected, int entryAliasNum)
        {
            if (MugshotValid())
                PopulateMugshot(character, localSelected, entryAliasNum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool MugshotValid() => Mugshots != null && Mugshots.Length != 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="localSelected"></param>
        /// <param name="entryAliasNum"></param>
        protected virtual void PopulateMugshot(Character character, bool localSelected, int entryAliasNum)
        {
            BacklogMugBind mug = Mugshots.First(f => f != null && f.charaName == character.FirstName);

            if (mug != null && mug.Mug_ON != null && mug.Mug_OFF != null)
            {
                mugshotImage.gameObject.SetActive(true);

                mugshotImage.sprite = localSelected ? mug.Mug_ON : mug.Mug_OFF;

                mugshotImage.transform.SetSiblingIndex(localSelected ? mugshotImage.transform.parent.childCount : 0);
            }

            if (mug == null || (character.FirstName == headmasterFirstName && entryAliasNum == 0))
                mugshotImage.gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Generate Mugshot List")]
        public void GenerateMugshotList()
        {
            if (characterDatabase != null)
            {
                Mugshots = new BacklogMugBind[characterDatabase.Characters.Count];

                for (int i = 0; i < characterDatabase.Characters.Count; i++)
                    Mugshots[i] = new(characterDatabase.Characters[i].FirstName, UnityEditor.Selection.objects);
            }
            else
                UnityEngine.Debug.LogWarning("ALERT: Must have Character Database assigned.");
        }
#endif

        /// <summary>
        ///
        /// </summary>
        [System.Serializable]
        sealed class BacklogMugBind
        {
            /// <summary>
            ///
            /// </summary>
            [Tooltip("")]
            [SerializeField] internal string charaName;

            /// <summary>
            ///
            /// </summary>
            [Tooltip("")]
            [SerializeField] internal Sprite Mug_ON;

            /// <summary>
            ///
            /// </summary>
            [Tooltip("")]
            [SerializeField] internal Sprite Mug_OFF;

            /// <summary>
            ///
            /// </summary>
            /// <param name="charaName"></param>
            /// <param name="objects"></param>
            internal BacklogMugBind(string charaName, Object[] objects)
            {
                this.charaName = charaName;

                UnityEngine.Debug.Log(nameof(BacklogMugBind) + ": Note when importing. " +
                    "Ensure you select all the headshot images of all characters so that they can be assigned.");

                UnityEngine.Debug.Log(nameof(BacklogMugBind) + ": Note when importing. Ensure that the names of the mug shot sprites " +
                    "contain the characters name, and the words \"Selected\" or \"Unselected\" for proper importing.");

                foreach (var sprite in objects)
                    if (sprite.name.Contains(charaName))
                        if (sprite.name.Contains("Unselected"))
                            Mug_OFF = (Sprite)sprite;
                        else
                            Mug_ON = (Sprite)sprite;
            }
        }
    }
}