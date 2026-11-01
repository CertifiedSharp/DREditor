//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// This was originally a core structure of LeotheDev's BacklogUI so credit to him too.
using DREditor.Services;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ScrollGridUI : MenuGroupBase
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
        [SerializeField] protected bool reverseOrder = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool singleTapInput = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int m_gridX = 8;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform[] slots;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Transform scrollBar;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector2 scrollBarBoundsTop = new(0, 318.1f);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector2 scrollBarBoundsBottom = new(0, -143);


        [Header("")]
        public UnityEvent OnHover;

        
        /// <summary>
        ///
        /// </summary>
        protected int totalObjectCount;

        /// <summary>
        ///
        /// </summary>
        protected int m_max;

        /// <summary>
        ///
        /// </summary>
        protected int m_min;

        /// <summary>
        ///
        /// </summary>
        protected int selectedSlotIndex;

        /// <summary>
        ///
        /// </summary>
        protected int globalIndex;

        /// <summary>
        ///
        /// </summary>
        protected IGridIconSlot[] gridSlots;

        /// <summary>
        ///
        /// </summary>
        protected Vector2 m_lastMoveInput = Vector2.zero;


        /// <summary>
        ///
        /// </summary>
        protected override void Start()
        {
            base.Start();

            gridSlots = slots.Select(slot => slot.GetComponent<IGridIconSlot>()).ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeScrollGrid()
        {
            if (scrollBar != null)
                InitializeScrollBar();

            InitializeGridValues();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeScrollBar() => scrollBar.gameObject.SetActive(true);

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeGridValues()
        {
            totalObjectCount = InitializeTotalObjectCount();

            m_min = 0;

            m_max = m_min + slots.Length - 1;

            selectedSlotIndex = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract int InitializeTotalObjectCount();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeControls() => navigate.action.performed += OnUINavigated;

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeControls() => navigate.action.performed -= OnUINavigated;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void OnUINavigated(CallbackContext ctx)
        {
            Vector2 moveInput = ctx.ReadValue<Vector2>();

            moveInput = reverseOrder? new Vector2(moveInput.y, moveInput.x) : moveInput;

            int newIndex = singleTapInput
                ? GetSingleTapIndex(moveInput)
                : GetContinuousIndex(moveInput);

            if (newIndex == selectedSlotIndex)
                return;

            UpdateGridValues(newIndex);

            OnHover?.Invoke();

            RegenerateUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual int GetSingleTapIndex(Vector2 moveInput)
        {
            int horizontalInput = Mathf.Clamp((int)moveInput.x, -1, 1);

            int verticalInput = Mathf.Clamp((int)moveInput.y, -1, 1);

            bool horizontalTriggered = horizontalInput != 0 && horizontalInput != Mathf.Clamp((int)m_lastMoveInput.x, -1, 1);

            bool verticalTriggered = verticalInput != 0 && verticalInput != Mathf.Clamp((int)m_lastMoveInput.y, -1, 1);

            m_lastMoveInput = moveInput;

            if (!horizontalTriggered && !verticalTriggered)
                return selectedSlotIndex;

            return verticalTriggered
                ? GetVerticalIndex(verticalInput)
                : GetHorizontalIndex(horizontalInput);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual int GetContinuousIndex(Vector2 moveInput)
        {
            int verticalInput = (int)moveInput.y;

            return verticalInput != 0
                ? GetVerticalIndex(Mathf.Clamp(verticalInput, -1, 1))
                : GetHorizontalIndex(Mathf.Clamp((int)moveInput.x, -1, 1));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual int GetHorizontalIndex(int horizontalInput) 
            => Mathf.Clamp(selectedSlotIndex + horizontalInput, 0, totalObjectCount - 1);

        /// <summary>
        ///
        /// </summary>
        protected virtual int GetVerticalIndex(int verticalInput)
        {
            int newIndex = selectedSlotIndex + m_gridX * verticalInput * -1;

            if (newIndex < 0)
                return selectedSlotIndex;

            return Mathf.Clamp(newIndex, 0, totalObjectCount - 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newIndex"></param>
        protected virtual void UpdateGridValues(int newIndex)
        {
            selectedSlotIndex = newIndex;

            if (selectedSlotIndex > m_max)
            {
                m_max += m_gridX;

                m_min += m_gridX;
            }

            if (selectedSlotIndex < m_min)
            {
                m_max -= m_gridX;

                m_min -= m_gridX;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateScrollBarPosition()
        {
            float scrollRatio = selectedSlotIndex / Mathf.Max((float)totalObjectCount - 1, 1);

            scrollRatio = reverseOrder ? 1f - scrollRatio : scrollRatio;

            scrollBar.transform.localPosition = new
                Vector3(scrollBar.transform.localPosition.x, Mathf.Lerp(scrollBarBoundsTop.y, scrollBarBoundsBottom.y, scrollRatio), 0);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RegenerateUI()
        {
            int selectedIndex = selectedSlotIndex - m_min;

            for (int i = 0; i < slots.Length; i++)
            {
                globalIndex = m_min + i;

                bool atEndOfList = i >= totalObjectCount || globalIndex >= totalObjectCount;

                if (!atEndOfList)
                    UpdateSlotState(selectedIndex == i);
                else
                    ClearSlotState();
            }

            if (scrollBar != null)
                UpdateScrollBarPosition();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selected"></param>
        protected virtual void UpdateSlotState(bool selected)
        {
            gridSlots[GetSlotIndex()].UpdateSlot(globalIndex, selected);

            if (selected)
                OnSlotHovered();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSlotHovered() { }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearSlotState() => gridSlots[GetSlotIndex()].ClearSlotState();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int GetSlotIndex()
        {
            int index = globalIndex - m_min;

            return reverseOrder ? (slots.Length - 1 - index) : index;
        }


        /// <inheritdoc/>
        public override void ChangeGroup(MenuGroupBase group)
        {
            if (Changing)
            {
                UnityEngine.Debug.LogWarning("ALERT: Menu's are still changing!");

                return;
            }

            ServiceLocator.GetService<MenuState>().SetChanging(true);

            EventSystem.current.SetSelectedGameObject(null);

            Hide();

            group.Show();
        }
    }
}