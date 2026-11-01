//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.TrialDialogueHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessor) +
        "\n\n")]
    public class MultiLineProcessor : DialogueComponent, ILineProcessor
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> lineProcessorComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("lineProcessorComponents")]
        [SerializeField] protected List<DialogueComponent> multiLineProcessorComponents = new();

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<ILineProcessor> lineProcessors;

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<ILineProcessor> multiLineProcessors;

        /// <summary>
        ///
        /// </summary>
        protected List<MultiPanel> multiPanels;

        /// <summary>
        ///
        /// </summary>
        protected Line currentLine;

        /// <summary>
        ///
        /// </summary>
        protected IMultiPanelHolder multiLine;


        protected virtual void Awake()
        {
            lineProcessors = lineProcessorComponents.Cast<ILineProcessor>();

            multiLineProcessors = multiLineProcessorComponents.Cast<ILineProcessor>();
        }

        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine)
        {
            this.currentLine = currentLine;

            if (LineIsMultiPanelHolder(out multiLine))
                ProcessMultiLine();
            else if (StalePanelsAvailable())
                ProcessNormalLine();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiLine"></param>
        /// <returns></returns>
        protected virtual bool LineIsMultiPanelHolder(out IMultiPanelHolder multiLine)
        {
            multiLine = currentLine is IMultiPanelHolder multi ? multi : null;

            return multiLine != null;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ProcessMultiLine()
        {
            InitializeMultiPanels();

            ProcessLineByCondition(OnFirstSpeaker());

            ProcessMultiPanels();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeMultiPanels() => multiPanels = multiLine?.GetMultiPanels();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnFirstSpeaker()
        {
            if (currentLine is IMultiPanelHolder mLine && mLine.PanelNumber != 0)
                return false;

            return !MultiPanelsHasFirstIndex();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="currentLine"></param>
        protected virtual void ProcessLineByCondition(bool condition, Line currentLine = null)
        {
            currentLine ??= this.currentLine;

            var currentLineProcessors = condition ? lineProcessors : multiLineProcessors;

            ProcessLine(currentLineProcessors, currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="processors"></param>
        /// <param name="line"></param>
        protected virtual void ProcessLine(IEnumerable<ILineProcessor> processors, Line line)
        {
            foreach (ILineProcessor processor in processors)
                processor?.LineProcess(line);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ProcessMultiPanels()
        {
            foreach (var panel in multiPanels)
                ProcessLineByCondition(panel.panelNum == 0, panel?.ConvertToLine());
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool StalePanelsAvailable() => multiPanels != null && multiPanels.Count > 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ProcessNormalLine()
        {
            if (TryGetPanelMatchingSpeaker(out MultiPanel foundPanel))
                ProcessLineByCondition(foundPanel.panelNum == 0);
            else
                ProcessLineByCondition(!MultiPanelsHasFirstIndex());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="foundPanel"></param>
        /// <returns></returns>
        protected virtual bool TryGetPanelMatchingSpeaker(out MultiPanel foundPanel)
        {
            foundPanel = null;

            foreach (var panel in multiPanels)
                if (panel.Speaker == currentLine.Speaker)
                    foundPanel = panel;

            return foundPanel != null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool MultiPanelsHasFirstIndex()
        {
            foreach (var panel in multiPanels)
                if (panel.panelNum == 0)
                    return true;

            return false;
        }
    }
}