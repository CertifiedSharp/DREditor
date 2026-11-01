//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Mechanics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDebateLineManager) +
        "\n\n")]
    public class DebateLineManager : MonoBehaviour, IDebateLineManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDebate = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool bufferFrameBetweenProcesses = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LineProcessComponents = new();


        /// <summary>
        ///
        /// </summary>
        protected IPanelManager panelManager;

        /// <summary>
        ///
        /// </summary>
        protected IPhraseResetManager resetManager;

        /// <summary>
        ///
        /// </summary>
        protected IPanelLineListener[] lineListeners;

        /// <summary>
        ///
        /// </summary>
        protected IPanelCounter panelCounter;

        /// <summary>
        ///
        /// </summary>
        protected Panel currentPanel;


        protected virtual void Start()
        {
            var allComponents = transform.root.GetComponentsInChildren<Component>();

            lineListeners = allComponents.OfType<IPanelLineListener>().ToArray();

            panelCounter = allComponents.OfType<IPanelCounter>().FirstOrDefault();

            panelManager = allComponents.OfType<IPanelManager>().FirstOrDefault();

            resetManager = allComponents.OfType<IPhraseResetManager>().FirstOrDefault();
        }


        /// <inheritdoc/>
        public virtual Panel GetCurrentPanel() => currentPanel;

        /// <inheritdoc/>
        public virtual IEnumerator PlayLines<T>(List<T> panels) where T : Panel
        {
            for (int i = 0; i < panels.Count; i++)
            {
                panelCounter?.NextPanelCounter(i);

                yield return PlayPanel(panels[i]);
            }

            resetManager.ResetPhrases();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        protected virtual IEnumerator PlayPanel(Panel panel)
        {
            currentPanel = panel;

            InvokePanelListeners();

            resetManager.ResetPhrases();

            yield return InvokeLineProcesses(panel);

            yield return panelManager.PlayPanel(panel);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokePanelListeners()
        {
            foreach (IPanelLineListener lineListener in lineListeners)
                lineListener?.SetCurrentLine(currentPanel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual IEnumerator InvokeLineProcesses(Line currentLine)
        {
            foreach (var processor in LineProcessComponents)
            {
                if (processor is ILineProcessor lineProcessor)
                    lineProcessor?.LineProcess(currentLine);

                if (processor is ILineProcessorRoutine lineRoutine)
                    yield return lineRoutine?.LineProcessRoutine(currentLine);

                if (bufferFrameBetweenProcesses)
                    yield return null;
            }

            yield break;
        }


#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        public virtual void PlayTestPanel<T>(T panel) where T : Panel
        {
            inDebate.SetValue(true);

            StartCoroutine(PlayPanel(panel));
        }
#endif
    }
}