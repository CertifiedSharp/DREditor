//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ScrollDescription : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference navigate = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected RectTransform maskBox = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform controlBox = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text textBox = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ScrollDirection inputDirection = ScrollDirection.Horizontal;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ScrollDirection scrollUIDirection = ScrollDirection.Vertical;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [SerializeField] protected float scrollValue = 5f;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [SerializeField] protected GameObject scrollControlUI = null;


        /// <summary>
        ///
        /// </summary>
        protected Canvas canvas = null;

        /// <summary>
        ///
        /// </summary>
        protected bool textGreaterThanMaskBounds = false;

        /// <summary>
        ///
        /// </summary>
        protected Vector2 stickDirection = Vector2.zero;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => textBox.OnPreRenderText += CheckScrollableOnTextChange;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => textBox.OnPreRenderText -= CheckScrollableOnTextChange;

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => canvas = GetComponentInParent<Canvas>();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Update() => CheckScrollInput();


        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckScrollInput()
        {
            if (EvaluateScrollUI())
                ScrollUI();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool EvaluateScrollUI()
        {
            if (!canvas.enabled)
                textGreaterThanMaskBounds = false;

            if (!textGreaterThanMaskBounds || !navigate.action.IsPressed())
                return false;

            stickDirection = navigate.action.ReadValue<Vector2>();

            bool isInputDirection = InputAxisValue(stickDirection) != 0;

            bool notDiagonal = InverseAxisValue(stickDirection, inputDirection) == 0;

            return textGreaterThanMaskBounds && isInputDirection && notDiagonal;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ScrollUI()
        {
            if (InputAxisValue(stickDirection) > 0 && OnTopMaskEdge()) 
                ScrollTextBoxUI(scrollValue);// Move Up/Right

            if (InputAxisValue(stickDirection) < 0 && OnBottomMaskEdge()) 
                ScrollTextBoxUI(scrollValue * -1);// Move Down/Left
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnTopMaskEdge()
            => !(SubtractMovePositionSize(textBox.rectTransform) + scrollValue >= SubtractMovePositionSize(maskBox));

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnBottomMaskEdge()
            => !(ScrollAxisValue(textBox.rectTransform.position) - scrollValue <= ScrollAxisValue(maskBox.position));

        /// <summary>
        ///
        /// </summary>
        /// <param name="moveBoxAmount"></param>
        protected virtual void ScrollTextBoxUI(float moveBoxAmount)
        {
            var xPos = scrollUIDirection == ScrollDirection.Horizontal ? controlBox.position.x + moveBoxAmount : controlBox.position.x;

            var yPos = scrollUIDirection == ScrollDirection.Vertical ? controlBox.position.y + moveBoxAmount : controlBox.position.y;

            controlBox.position = new Vector3(xPos, yPos, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textInfo"></param>
        protected virtual void CheckScrollableOnTextChange(TMP_TextInfo textInfo)
        {
            controlBox.position = maskBox.position;

            textGreaterThanMaskBounds = ScrollAxisValue(textBox.rectTransform.sizeDelta) > ScrollAxisValue(maskBox.sizeDelta);

            StartCoroutine(BufferScrollControlsActive());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual IEnumerator BufferScrollControlsActive()
        {
            yield return new WaitForEndOfFrame();

            yield return null;

            if (scrollControlUI != null)
                scrollControlUI.SetActive(textGreaterThanMaskBounds);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        protected virtual float InputAxisValue(Vector2 dir) => AxisValue(dir, inputDirection);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        protected virtual float ScrollAxisValue(Vector2 dir) => AxisValue(dir, scrollUIDirection);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected float AxisValue(Vector2 dir, ScrollDirection direction)
            => direction == ScrollDirection.Horizontal ? dir.x : dir.y;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected float InverseAxisValue(Vector2 dir, ScrollDirection direction)
            => direction == ScrollDirection.Horizontal ? dir.y : dir.x;

        /// <summary>
        ///
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        protected float SubtractMovePositionSize(RectTransform box) => ScrollAxisValue(box.position) - ScrollAxisValue(box.sizeDelta);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public enum ScrollDirection
    {
        Horizontal, Vertical
    }
}