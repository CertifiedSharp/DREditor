//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.AddOns
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [RequireComponent(typeof(Animator))]
    public class AnimatorSetAllBools : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> boolStrings = new();


        /// <summary>
        ///
        /// </summary>
        protected Animator animator = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => animator = GetComponent<Animator>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <remarks></remarks>
        public virtual void SetAll(bool to)
        {
            foreach (string s in boolStrings)
                animator.SetBool(s, to);
        }
    }
}