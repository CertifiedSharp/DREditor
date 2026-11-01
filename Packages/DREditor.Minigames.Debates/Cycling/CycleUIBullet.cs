//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CycleUIBulletBase) +
        "\n\n")]
    public class CycleUIBullet : CycleUIBulletBase
    {
        /// <summary>
        ///
        /// </summary>
        protected ICycleUIStateComponent[] stateComponents;

        /// <summary>
        ///
        /// </summary>
        protected ICycleUITextComponent[] textComponents;


        protected virtual void Awake()
        {
            stateComponents = GetComponents<ICycleUIStateComponent>();

            textComponents = GetComponents<ICycleUITextComponent>();
        }


        /// <inheritdoc/>
        public override void UpdateBulletText(string title)
        {
            if (textComponents != null)
                foreach (var item in textComponents)
                    item?.UpdateBulletText(title);
        }

        /// <inheritdoc/>
        public override void DisplayBulletActive(bool isActive)
        {
            if (stateComponents != null)
                foreach (var item in stateComponents)
                    item?.DisplayBulletActive(isActive);
        }
    }
}