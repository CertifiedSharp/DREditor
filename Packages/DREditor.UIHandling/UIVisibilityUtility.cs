//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public static class UIVisibilityUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="visibleUI"></param>
        /// <param name="toBeVisible"></param>
        public static void SetVisibility(this IUIVisibility visibleUI, bool toBeVisible)
        {
            if (toBeVisible)
                visibleUI.Show();
            else
                visibleUI.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="visibleUI"></param>
        /// <param name="visibilityState"></param>
        /// <returns></returns>
        public static IEnumerator SetVisibilityRoutine(this IUIVisibility visibleUI, bool visibilityState)
        {
            visibleUI.SetVisibility(visibilityState);

            yield return visibleUI.WaitForChangeToComplete();

            yield break;
        }
    }
}