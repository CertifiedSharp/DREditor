//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    public class TextDisplayDebugger : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected bool debugging = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Line> debugLines = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueComponent lineDisplayerComponent;


        /// <summary>
        ///
        /// </summary>
        protected ILineTextDisplayRoutine lineTextDisplayer;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => lineTextDisplayer = (ILineTextDisplayRoutine)lineDisplayerComponent;

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => StartCoroutine(LoopDebugDisplayLines());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator LoopDebugDisplayLines()
        {
            while (debugging)
                yield return DisplayLines();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator DisplayLines()
        {
            foreach(var line in debugLines)
            {
                yield return lineTextDisplayer.LineTextDisplay(line);

                yield return null;

                while (!submit.action.triggered && line.Text != "")
                    yield return null;
            }
        }
    }
}