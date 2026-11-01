//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ICycleUITextComponent) +
        "\n\n")]
    public class CycleUIText : BulletTextHandler, ICycleUITextComponent
    {
        /// <summary>
        ///
        /// </summary>
        protected string title;

        /// <inheritdoc/>
        public virtual void UpdateBulletText(string title)
        {
            this.title = title;

            SetBulletText();

            StartCoroutine(SetFontRoutine());
        }

        /// <inheritdoc/>
        protected override void SetBulletText() => bulletTMP.text = title;
    }
}