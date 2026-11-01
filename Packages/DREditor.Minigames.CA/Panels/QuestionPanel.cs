// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using UnityEngine;

namespace DREditor.Minigames.CA.Panels
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(QuestionPanelBase) +
        "\n\n")]
    public class QuestionPanel : QuestionPanelBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent topPanelVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent bottomPanelVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float yPositionThreshold = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 miniParentScale = new(0.58f, 0.58f, 1);


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SpriteMask spriteMask = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int sortOrderOnComplete = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int sortOrderOnCompleteMini = 6;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SpriteRenderer symbolRenderer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite checkMark;


        /// <summary>
        ///
        /// </summary>
        protected UIVisibilityComponent panelVisibility = null;


        protected virtual void Awake()
        {
            topPanelVisibility.Hide();

            bottomPanelVisibility.Hide();
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            InitializeQuestionDirection();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeQuestionDirection() =>
            panelVisibility = AboveYThreshold() ? topPanelVisibility : bottomPanelVisibility;

        /// <summary>
        ///
        /// </summary>
        protected virtual bool AboveYThreshold() => transform.localPosition.y > yPositionThreshold;


        protected virtual void OnTriggerEnter2D(Collider2D collision) => panelVisibility.Show();

        protected virtual void OnTriggerExit2D(Collider2D collision) => panelVisibility.Hide();


        /// <inheritdoc/>
        public override void OnPanelCorrect()
        {
            panelVisibility.Hide();

            int correctSortOrder = transform.parent.localScale.x != miniParentScale.x
                ? sortOrderOnComplete
                : sortOrderOnCompleteMini;

            spriteMask.frontSortingOrder = correctSortOrder;

            symbolRenderer.sortingOrder = correctSortOrder + 1;

            symbolRenderer.sprite = checkMark;

            GetComponent<PolygonCollider2D>().enabled = false;
        }

        /// <inheritdoc/>
        public override void OnPreview() => DisableNonSymbolChildren();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableNonSymbolChildren()
        {
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).gameObject != symbolRenderer.gameObject)
                    transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}