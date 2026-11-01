//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.ItemRolling;
using DREditor.Processables;

namespace DREditor.GachaSurface
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SubmittableAnimationSurfaceComponent) + ", " +
        nameof(ISubmittable) + ", " + nameof(IStartUpable) + ", " + nameof(IItemRollerResponse) +
        "\n\n")]
    public class ItemRolledAnimationSurface : SubmittableAnimationSurfaceComponent, IItemRollerResponse
    {
        /// <summary>
        ///
        /// </summary>
        protected IItemRoller itemRoller;


        /// <inheritdoc/>
        public virtual void InitializeForResponse(IItemRoller itemRoller) => this.itemRoller = itemRoller;


        /// <inheritdoc/>
        protected override void OnSubmit()
        {
            if (RolledItemValid())
                base.OnSubmit();
            else
                InvokeCompleted();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool RolledItemValid() => itemRoller.TryGetRolledObject(out object _);
    }
}