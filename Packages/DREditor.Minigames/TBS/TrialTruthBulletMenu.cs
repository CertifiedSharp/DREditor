//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UI;
using DREditor.UI.Menus;

namespace DREditor.Minigames.TBS
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(MenuModifier) + ", " + nameof(TruthBulletMenu) +
        "\n\n")]
    public class TrialTruthBulletMenu : TruthBulletMenu
    {
        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            InitializeEvidence();

            LoadAllBulletsOnEmpty();
        }
    }
}