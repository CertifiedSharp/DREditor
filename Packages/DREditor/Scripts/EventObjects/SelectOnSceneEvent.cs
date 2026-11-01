//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine.EventSystems;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) +
        "\n\n")]
    public class SelectOnSceneEvent : SceneEventListener
    {
        protected override void OnResponseInvoked()
        {
            base.OnResponseInvoked();

            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}