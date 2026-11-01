//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<MapHoverSelector>) + " for type: " + nameof(MapHoverSelector) +
        "\n\n")]
    public class MapHoverSelector : ServiceHolder<MapHoverSelector>
    {
        /// <summary>
        ///
        /// </summary>
        [Space]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image selectionImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform reticleTransform = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject fastTravelUIObject = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFastTravel = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float hoverDuration = 1;


        [Header("")]
        [SerializeField] protected UnityEvent onHoverComplete = null;


        /// <summary>
        ///
        /// </summary>
        protected PlaceManager currentPlace = null;

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected Vector3 lastPosition = Vector3.zero;


        /// <summary>
        ///
        /// </summary>
        /// <param name="place"></param>
        public virtual void AnimateToHoveredPlace(PlaceManager place) => StartCoroutine(WaitUntilMapLoadedToAnimate(place));

        /// <summary>
        ///
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        protected virtual IEnumerator WaitUntilMapLoadedToAnimate(PlaceManager place)
        {
            yield return new WaitUntil(() => ServiceLocator.HasService<MapAreaManager>()
            && ServiceLocator.GetService<MapAreaManager>() != null);

            AnimateToPlace(place);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="place"></param>
        protected virtual void AnimateToPlace(PlaceManager place)
        {
            if (!ServiceLocator.HasService<MapAreaManager>() || place == null)
                return;

            GameObject areaGameObject = ServiceLocator.GetService<MapAreaManager>().gameObject;

            if (selectionImage != null && EventSystem.current.currentSelectedGameObject != null)
                selectionImage.transform.position = EventSystem.current.currentSelectedGameObject.transform.position;

            Vector3 toPosition = reticleTransform.position + (areaGameObject.transform.position - place.transform.position);

            if (currentPlace != null && lastPosition == toPosition)
            {
                lastPosition = areaGameObject.transform.position;

                return;
            }

            AnimateToObject(areaGameObject, toPosition, place);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="areaGameObject"></param>
        /// <param name="toPosition"></param>
        /// <param name="place"></param>
        protected virtual void AnimateToObject(GameObject areaGameObject, Vector3 toPosition, PlaceManager place)
        {
#if DOTWEEN
            areaGameObject.transform.DOKill();

            areaGameObject.transform.DOMove(toPosition, hoverDuration)
                .SetUpdate(true)
                .OnComplete(InvokeHoverComplete);
#endif
            if (currentPlace != null)
                currentPlace.Deselect();

            place.Select();

            lastPosition = toPosition;

            currentPlace = place;

            fastTravelUIObject.SetActive(PlaceIsFastTravelable() && canFastTravel.Value);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeHoverComplete() => onHoverComplete?.Invoke();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual PlaceManager GetCurrentPlace() => currentPlace;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool PlaceIsFastTravelable() => currentPlace != null && currentPlace.Data.fastTravelable &&
                currentPlace.GetEntryInstance().enableFastTravelForThisEntryInstance;
    }
}