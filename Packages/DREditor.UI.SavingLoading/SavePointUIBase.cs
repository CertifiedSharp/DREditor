//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SavePointUIBase : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool roomIsSavePoint = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase savePointGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SaveLoadMenuBase saveLoadMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ReturnToMenuHandler returnToMenuHandler = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        protected bool hasLeaveAbility = false;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            if (isLoadingFile.Value)
                Response.AddListener(StartSavePointTransition);
            else if (savePointGroup != null)
                Response.AddListener(ShowSavePointUI);

            if (!roomIsSavePoint && saveLoadMenu != null)
                saveLoadMenu.AddOnSaveListener(MoveToSavePointOnBack);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            Response.RemoveListener(ShowSavePointUI);

            Response.RemoveListener(StartSavePointTransition);

            if (!roomIsSavePoint && saveLoadMenu != null)
                saveLoadMenu.RemoveOnSaveListener(MoveToSavePointOnBack);

            if (hasLeaveAbility && saveLoadMenu != null)
                saveLoadMenu.GetComponentInChildren<MenuGroupBase>().OnBack.RemoveAllListeners();
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void ExecuteValidSavePoint()
        {
            if (SavePointIsValid())
                LoadSavePoint();
            else if (roomIsSavePoint)
                return;
            else
                returnToMenuHandler.ReturnToMenu();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool SavePointIsValid();

        /// <summary>
        ///
        /// </summary>
        protected abstract void LoadSavePoint();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowSavePointUI() => savePointGroup.Show();

        /// <summary>
        ///
        /// </summary>
        protected abstract void StartSavePointTransition();

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveToSavePointOnBack()
        {
            if (!hasLeaveAbility)
            {
                hasLeaveAbility = true;

                MenuGroupBase menuGroup = saveLoadMenu.GetComponentInChildren<MenuGroupBase>();

                menuGroup.OnBack.AddListener(menuGroup.Hide);

                menuGroup.OnBack.AddListener(StartSavePointTransition);
            }
        }
    }
}