//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
	[ClassHeader("")]
    public class RailTextUpdater : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected TMP_Text railText;

        /// <summary>
        ///
        /// </summary>
        protected string originalText;


        protected virtual void Awake() => railText = GetComponent<TMP_Text>();


        public virtual void UpdateRailText(string updatedText)
        {
            StopAllCoroutines();

            if (!string.IsNullOrEmpty(originalText) && railText.text != originalText)
                railText.text = originalText;
            else
                originalText = railText.text;

            if (railText.text != updatedText)
                StartCoroutine(UpdateRailText(railText.text, updatedText));
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="current"></param>
        /// <param name="replacement"></param>
        protected virtual IEnumerator UpdateRailText(string current, string replacement)
        {
            if (railText.text == current)
            {
                yield return RemoveRailText(current);

                yield return AddRailText(replacement);

                originalText = railText.text;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="remove"></param>
        protected virtual IEnumerator RemoveRailText(string remove)
        {
            for (int i = 0; i < remove.Length; i++)
            {
                railText.text = railText.text[..^1];

                yield return new WaitForSeconds(Time.unscaledDeltaTime);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="add"></param>
        protected virtual IEnumerator AddRailText(string add)
        {
            for (int i = 0; i < add.Length; i++)
            {
                railText.text += add[i];

                yield return new WaitForSeconds(Time.unscaledDeltaTime);
            }
        }
    }
}