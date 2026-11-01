//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.Purchasing
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class FollowSelectedObject : MonoBehaviour
    {
        protected virtual void LateUpdate()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                transform.position = EventSystem.current.currentSelectedGameObject.transform.position;
        }
    }
}