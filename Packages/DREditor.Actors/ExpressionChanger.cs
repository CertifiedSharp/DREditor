//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Actors
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ExpressionChanger : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="expressionMaterial"></param>
        /// <param name="expressionSprite"></param>
        public abstract void FastForwardExpressionChange(Material expressionMaterial, Sprite expressionSprite);

        /// <summary>
        ///
        /// </summary>
        /// <param name="expressionMaterial"></param>
        /// <param name="expressionSprite"></param>
        public abstract IEnumerator ChangeExpressionProcess(Material expressionMaterial, Sprite expressionSprite);
    }
}