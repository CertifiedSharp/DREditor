//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(SceneEventListener) +
        "\n\n")]
    public class ClearMapInstanceData : SceneEventListener
    {
        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            Response.AddListener(ClearManagerInstanceData);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            Response.RemoveListener(ClearManagerInstanceData);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearManagerInstanceData() => ServiceLocator.GetService<IMapManager>().SetInstanceData(null);
    }
}