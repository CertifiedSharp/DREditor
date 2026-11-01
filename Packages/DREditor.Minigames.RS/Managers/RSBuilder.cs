//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.Minigames.Debates;
using DREditor.Services;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Minigames/RS", fileName = "RS Builder")]
    public class RSBuilder : DebateBuilder<RSGroup>, IDebateSequence, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        public List<int> panelCharacterNumbers = new();

        /// <summary>
        ///
        /// </summary>
        public GameObject interjectObject;

        /// <summary>
        ///
        /// </summary>
        public List<MultiPanel> introPanels = new();

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("StartingRSDialogue")]
        public MultiTrialDialogue startingRSDialogue;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("DefaultRetreatDialogue")]
        public MultiTrialDialogue defaultRetreatDialogue;


        /// <summary>
        ///
        /// </summary>
        protected int sectionIndex;


        /// <summary>
        ///
        /// </summary>
        public int SectionIndex
        {
            get => sectionIndex;

            set
            {
                if (Lines != null && value >= 0 && value < Lines.Count)
                    sectionIndex = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public List<int> PanelCharacterNumbers { get => panelCharacterNumbers; }

        /// <summary>
        ///
        /// </summary>
        public GameObject InterjectObject { get => interjectObject; set => interjectObject = value; }

        /// <summary>
        ///
        /// </summary>
        public MultiTrialDialogue StartingRSDialogue { get => startingRSDialogue; set => startingRSDialogue = value; }

        /// <summary>
        ///
        /// </summary>
        public DialogueCore DefaultRetreatDialogue
        { get => defaultRetreatDialogue; set => defaultRetreatDialogue = (MultiTrialDialogue)value; }

        /// <inheritdoc/>
        public override int LineCount
        {
            get
            {
                if (Lines == null || SectionIndex < 0 || SectionIndex >= Lines.Count)
                    return 0;

                return Lines[SectionIndex]?.LineCount ?? 0;
            }
        }


        /// <inheritdoc/>
        public override Line GetLine(int index)
        {
            if (Lines == null || SectionIndex < 0 || SectionIndex >= Lines.Count)
                return null;

            return Lines[SectionIndex]?.GetLine(index);
        }

        /// <inheritdoc/>
        public override List<T2> GetLines<T2>()
        {
            if (Lines == null || SectionIndex < 0 || SectionIndex >= Lines.Count)
                return base.GetLines<T2>();

            if (Lines[SectionIndex].GetLines<T2>() is List<T2> castedLines)
                return castedLines;
            else
                return base.GetLines<T2>();
        }

        /// <inheritdoc/>
        public override void Convert()
        {
            foreach (var section in Lines)
                section.Convert();
        }

        /// <inheritdoc/>
        public override bool Scan(out string log)
        {
            log = "";

            bool lineIssue = false;

            if (StartingRSDialogue == null)
                log += $"\n- Does not have Starting Dialogue!";

            for (int i = 0; i < Lines.Count; i++)
            {
                var section = Lines[i];

                if (section is IDebateScanner scannable && scannable.Scan(out string lineResult))
                {
                    if (i > 0 && section.RetreatDialogue == null && defaultRetreatDialogue == null)
                        lineResult += " Missing Retreat Dialogue!";

                    if (lineResult != "")
                        log += $"\n- Issue on Panel Group index: {i} {lineResult}";

                    lineIssue = true;
                }
            }

            log = log != "" ? name + " contains issues: " + log : log;

            return StartingRSDialogue == null || lineIssue;
        }

        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue() => 
            ServiceLocator.GetService<IDialogueManager>().GetManagerComponent<DialogueLoadIdentifier>()
            .CheckDialogue(startingRSDialogue);

#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        public List<int> assignedPanelNumbers = new();

        /// <summary>
        ///
        /// </summary>
        protected bool autoAssignPanelNums = true;

        /// <summary>
        ///
        /// </summary>
        protected bool showIntroPanels = false;


        /// <summary>
        ///
        /// </summary>
        public bool AutoAssignPanelNums { get => autoAssignPanelNums; set => autoAssignPanelNums = value; }

        /// <summary>
        ///
        /// </summary>
        public bool ShowIntroPanels { get => showIntroPanels; set => showIntroPanels = value; }


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lines"></param>
        /// <remarks></remarks>
        public virtual void AssignPanelNums<T>(List<T> lines) where T : LineBase
        {
            if (assignedPanelNumbers == null || panelCharacterNumbers == null || assignedPanelNumbers.Count < 2)
                return;

            for (int i = 0; i < lines.Count; i++)
                if (!TryAssignPanelNum(lines, i))
                    continue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lines"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool TryAssignPanelNum<T>(List<T> lines, int i) where T : LineBase
        {
            if (lines[i] == null || !panelCharacterNumbers.Contains(lines[i].SpeakerNumber) || 
                lines[i] is not IMultiPanelHolder panelHolder)
                return false;

            int charIndex = panelCharacterNumbers.IndexOf(lines[i].SpeakerNumber);

            if (charIndex >= 0 && charIndex < assignedPanelNumbers.Count)
            {
                int autoNum = assignedPanelNumbers[charIndex];

                if (autoNum != -1)
                    panelHolder.PanelNumber = autoNum;
            }

            return AssignedMultiPanelNums(panelHolder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panelHolder"></param>
        /// <returns></returns>
        protected virtual bool AssignedMultiPanelNums(IMultiPanelHolder panelHolder)
        {
            var multiPanels = panelHolder.GetMultiPanels();

            if (multiPanels == null)
                return false;

            foreach (var panel in multiPanels)
            {
                if (panel == null || !panelCharacterNumbers.Contains(panel.SpeakerNumber))
                    continue;

                int index = panelCharacterNumbers.IndexOf(panel.SpeakerNumber);

                if (index >= 0 && index < assignedPanelNumbers.Count)
                {
                    int autoPanelNum = assignedPanelNumbers[index];

                    if (autoPanelNum != -1)
                        panel.SetPanelNum(autoPanelNum);
                }
            }

            return true;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="foundChars"></param>
        /// <param name="panel"></param>
        /// <param name="baseLine"></param>
        public virtual void ImportSpeakers(List<int> foundChars, LineBase panel, LineBase baseLine)
        {
            ImportSpeaker(foundChars, panel, panel);

            if (panel is IMultiPanelHolder panelHolder)
                foreach (var multiPanel in panelHolder.GetMultiPanels())
                    ImportSpeaker(foundChars, panel, multiPanel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="foundChars"></param>
        /// <param name="panel"></param>
        /// <param name="baseLine"></param>
        protected virtual void ImportSpeaker(List<int> foundChars, LineBase panel, LineBase baseLine)
        {
            if (baseLine == null)
                return;

            if (!foundChars.Contains(baseLine.SpeakerNumber))
            {
                foundChars.Add(baseLine.SpeakerNumber);

                if (!panelCharacterNumbers.Contains(baseLine.SpeakerNumber))
                {
                    panelCharacterNumbers.Add(baseLine.SpeakerNumber);

                    assignedPanelNumbers.Add(assignedPanelNumbers.Count);
                }
            }
        }
#endif
    }
}