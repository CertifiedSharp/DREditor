//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine.EventSystems;

namespace DREditor.EventObjects
{
    public class SelectObjectOnSceneEvent : SceneEventListener
    {
        protected override void OnResponseInvoked()
        {
            base.OnResponseInvoked();

            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}