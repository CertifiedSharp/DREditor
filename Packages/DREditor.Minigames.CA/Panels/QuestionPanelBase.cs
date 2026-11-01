// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Minigames.CA.Panels
{
    /// <summary>
    ///
    /// </summary>
    public abstract class QuestionPanelBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text questionTMP = null;


        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("answer")]
        protected ClosingArgumentStockBase stock;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("questionText")]
        protected string question;


        /// <summary>
        ///
        /// </summary>
        /// <param name="stock"></param>
        public virtual void SetStock(ClosingArgumentStockBase stock) => this.stock = stock;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual Transform GetStockTransform() => stock.transform;

        /// <summary>
        ///
        /// </summary>
        /// <param name="question"></param>
        public virtual void SetQuestion(string question) => this.question = question;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => questionTMP.text = question;


        /// <summary>
        ///
        /// </summary>
        public abstract void OnPanelCorrect();

        /// <summary>
        ///
        /// </summary>
        public abstract void OnPreview();
    }
}