//Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Originally done by Willy Bee
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UI.RegulationRules
{
    /// <summary>
    ///
    /// </summary>
    public class RegulationDisplayer : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected Image ruleImg = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI ruleText = null;



        /// <summary>
        ///
        /// </summary>
        public virtual void DisplayRegulation(IRegulation regulation)
        {
            if (ruleImg != null)
                ruleImg.sprite = regulation.Image;

            if (ruleText != null)
                ruleText.text = regulation.Text;
        }
    }
}