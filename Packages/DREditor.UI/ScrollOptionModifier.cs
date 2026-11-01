//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MenuModifier) +
        "\n\n")]
    public class ScrollOptionModifier : MenuModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform parentTransform = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<ScrollOption> scrollOptions = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public virtual GameObject InstantiateScrollOption(GameObject go)
        {
            Transform parent = parentTransform != null ? parentTransform : transform;

            ScrollOption option = Instantiate(go, parent).GetComponent<ScrollOption>();

            ((SelectableMenuGroupBase)menu).selectables.Add(option.GetSelectable());

            scrollOptions.Add(option);

            return option.gameObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="optionAmountUsed"></param>
        public virtual void DisableUnused(int optionAmountUsed)
        {
            for (int i = 0; i < scrollOptions.Count; i++)
                if (i < optionAmountUsed)
                    continue;
                else
                    scrollOptions[i].DisableState();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="text"></param>
        public virtual void EnableOptionAt(int index, string text = "") => scrollOptions[index].EnableState(text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        public virtual void DisableOptionAt(int index) => scrollOptions[index].DisableState();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int OptionCount() => scrollOptions.Count;
    }
}