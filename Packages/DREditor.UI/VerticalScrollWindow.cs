//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(MenuModifier) +
        "\n\n")]
    public class VerticalScrollWindow : MenuModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform container = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GridLayoutGroup layout = null;

        /// <summary>
        ///
        /// </summary>
        [Divider]

        [Tooltip("")]
        [SerializeField] protected bool resetContainerOnShow = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setBoundsOnCommandOnly = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool addScrollWhenBoundsSet = false;


        /// <summary>
        ///
        /// </summary>
        protected float cellSize = 0f;

        /// <summary>
        ///
        /// </summary>
        protected float midPoint;

        /// <summary>
        ///
        /// </summary>
        protected float top;

        /// <summary>
        ///
        /// </summary>
        protected float bottom;

        /// <summary>
        ///
        /// </summary>
        protected float yScale;

        /// <summary>
        ///
        /// </summary>
        protected bool boundsSet = false;

        /// <summary>
        ///
        /// </summary>
        protected int resolutionX;

        /// <summary>
        ///
        /// </summary>
        protected int resolutionY;

        /// <summary>
        ///
        /// </summary>
        protected bool notCentered;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 screenChangeOrigin;

        /// <summary>
        ///
        /// </summary>
        protected bool scrollAdded = false;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 origin;


        /// <summary>
        ///
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            resolutionX = Screen.width;

            resolutionY = Screen.height;

            origin = container.localPosition;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            yScale = GetComponentInParent<Canvas>().scaleFactor;

            SetCellSize();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            menu.BeforeStart.AddListener(BeforeStartSetContainerBounds);

            if (!addScrollWhenBoundsSet)
                menu.StartEvents.AddListener(AddScroll);

            menu.EndEvents.AddListener(RemoveScroll);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            menu.BeforeStart.RemoveListener(BeforeStartSetContainerBounds);

            if (!addScrollWhenBoundsSet)
                menu.StartEvents.RemoveListener(AddScroll);

            menu.EndEvents.RemoveListener(RemoveScroll);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void AddScroll()
        {
            if (scrollAdded)
                return;

            scrollAdded = true;

            ServiceLocator.GetService<UIHandlerBase>().OnChange.AddListener(EvaluateScroll);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void RemoveScroll()
        {
            if (!scrollAdded)
                return;

            scrollAdded = false;

            ServiceLocator.GetService<UIHandlerBase>().OnChange.RemoveListener(EvaluateScroll);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateScroll()
        {
            if (!boundsSet)
                return;

            float yPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().position.y;

            if (yPosition <= bottom)
                ScrollMenuVertical(false);
            else if (yPosition >= top)
                ScrollMenuVertical(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="up"></param>
        protected virtual void ScrollMenuVertical(bool up)
        {
            Vector3 containerPosition = container.position;

            if (up)
                containerPosition.y -= cellSize;
            else
                containerPosition.y += cellSize;

            container.position = containerPosition;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Update()
        {
            if (resolutionX == Screen.width && resolutionY == Screen.height)
                return;
            else
                CheckScrollBounds();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckScrollBounds()
        {
            if (menu.isActive)
            {
                UnityEngine.Debug.LogWarning($"{ToString()}: SCREEN WIDTH CHANGED AND SET BOUNDS");

                UpdateContainerLocalPosition();

                AttemptSetBounds();
            }

            resolutionX = Screen.width;

            resolutionY = Screen.height;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateContainerLocalPosition()
        {
            notCentered = container.localPosition != Vector3.zero;

            if (notCentered)
                screenChangeOrigin = container.localPosition;

            container.localPosition = Vector2.zero;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void BeforeStartSetContainerBounds()
        {
            if (resetContainerOnShow)
                container.localPosition = origin;

            if (boundsSet)
                return;

            if (!setBoundsOnCommandOnly)
                AttemptSetBounds();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AttemptSetBounds()
        {
            if (!boundsSet)
                StartCoroutine(SetBounds());

            if (addScrollWhenBoundsSet)
                AddScroll();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator SetBounds()
        {
            Canvas canvas = GetComponentInParent<Canvas>();

            yield return new WaitUntil(() => canvas.enabled == true);

            Canvas.ForceUpdateCanvases();

            yScale = GetComponentInParent<Canvas>().scaleFactor;

            SetCellSize();

            SetRectPoints();

            boundsSet = true;

            if (notCentered)
                container.localPosition = screenChangeOrigin;

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetCellSize()
        {
            cellSize = layout.cellSize.y;

            cellSize *= yScale;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetRectPoints()
        {
            midPoint = container.position.y;

            top = midPoint + container.rect.height * yScale / 2;

            bottom = midPoint - container.rect.height * yScale / 2;
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void CommandSetContainerBounds()
        {
            if (boundsSet && addScrollWhenBoundsSet)
                AddScroll();

            if (boundsSet)
                return;

            AttemptSetBounds();
        }
    }
}