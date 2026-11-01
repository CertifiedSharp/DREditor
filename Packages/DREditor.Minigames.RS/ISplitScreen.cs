//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Minigames.Debates.Mechanics;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    public interface ISplitScreen : IPanelSendable
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        int CalculateDomination();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool WillAdvanceOnTie();

        /// <summary>
        ///
        /// </summary>
        /// <param name="damage"></param>
        void PlayerAttack(float damage);

        /// <summary>
        ///
        /// </summary>
        /// <param name="enemyIndex"></param>
        /// <param name="damage"></param>
        void EnemyAttack(int enemyIndex, float damage);
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        /// <param name="time"></param>
        /// <param name="ease"></param>
        void AnimateToResetState(float time = 1, Ease ease = Ease.Linear);
#else
        void AnimateToResetState();
#endif
    }
}