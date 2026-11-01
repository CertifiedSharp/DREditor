//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Progressing.Handling;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class KeepStateAfterObjective : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<PastPointData> states = new();


        /// <summary>
        ///
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            Response.AddListener(ApplyReleventState);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            Response.RemoveListener(ApplyReleventState);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyReleventState()
        {
            for (int i = states.Count-1; i >= 0; i--)
            {
                if (ServiceLocator.GetService<ProgressionManagerBase>().PassedPoint(states[i].chapterIndex, states[i].objective))
                {
                    ApplyStateData(states[i]);

                    return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        protected virtual void ApplyStateData(PastPointData state)
        {
            gameObject.transform.position = state.position;

            gameObject.transform.eulerAngles = state.rotation;

            gameObject.SetActive(state.active);
        }
    }
}