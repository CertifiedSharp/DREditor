//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Minigames.Debates;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class RSGroup : IDebateScanner, IConvertableDialogue, IDebateConverter
    {
        /// <summary>
        ///
        /// </summary>
        public int cutCount = 20;

        /// <summary>
        ///
        /// </summary>
        public List<RSPanel> Panels = new() { new() };

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("EndGroupDialogue")]
        public MultiTrialDialogue endGroupDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("RetreatDialogue")]
        public MultiTrialDialogue retreatDialogue = null;

        /// <summary>
        ///
        /// </summary>
        public List<RSPanel> ResponsePanels = new();


        /// <summary>
        ///
        /// </summary>
        public MultiTrialDialogue EndGroupDialogue { get => endGroupDialogue; set => endGroupDialogue = value; }

        /// <summary>
        ///
        /// </summary>
        public MultiTrialDialogue RetreatDialogue { get => retreatDialogue; set => retreatDialogue = value; }

        /// <summary>
        ///
        /// </summary>
        public int CutCount { get => cutCount; set => cutCount = value; }

        public int LineCount { get => Panels.Count; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="includeAllPhraseTypes"></param>
        /// <returns></returns>
        public virtual int GetSectionHitablePhraseCount(bool includeAllPhraseTypes = false)
        {
            int count = 0;

            foreach (var panel in Panels)
                foreach (var phrase in panel.Phrases)
                    if ((phrase is Phrase rsPhrase && !rsPhrase.Has<HittableModifierBase>()) || includeAllPhraseTypes)
                        count++;

            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="foundChars"></param>
        public virtual void ImportMultiPanels(List<int> foundChars)
        {
            foreach (var panel in Panels)
            {
                for (int i = 0; i < foundChars.Count; i++)
                {
                    if (foundChars[i] != panel.SpeakerNumber)
                    {
                        var multiPanel = new MultiPanel();

                        multiPanel.SetSpeakerNum(foundChars[i]);

                        multiPanel.CamAnimIdx = panel.CamAnimIdx;

                        panel.multiPanels.Add(multiPanel);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public virtual List<T2> GetResponsePanels<T2>() => GetSectionLines<T2>(ResponsePanels);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public virtual List<T2> GetLines<T2>() => GetSectionLines<T2>(Panels);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="panels"></param>
        /// <returns></returns>
        protected virtual List<T2> GetSectionLines<T2>(List<RSPanel> panels)
        {
            if (panels is List<T2> castedLines)
                return castedLines;
            else if (panels != null && typeof(T2).IsAssignableFrom(typeof(RSPanel)))
                return panels.Cast<T2>().ToList();
            else
                return null;
        }


        /// <inheritdoc/>
        public virtual Line GetLine(int index) => Panels[index] as Line ?? null;

        /// <inheritdoc/>
        public virtual void Convert()
        {
            foreach (var rpanel in ResponsePanels)
                foreach (var phrase in rpanel.Phrases)
                    phrase.Convert();

            foreach (var panel in Panels)
                foreach (var phrase in panel.Phrases)
                    phrase.Convert();
        }

        /// <inheritdoc/>
        public virtual bool Scan(out string log)
        {
            log = "";

            if (EndGroupDialogue == null)
                log += "\n Missing End Group Dialogue!";

            bool lineIssue = false;

            foreach (var panel in Panels)
                if (panel is IDebateScanner scannable && scannable.Scan(out string panelLog))
                {
                    log += $"\n Issue on Panel index: {Panels.IndexOf(panel)} {panelLog}";

                    lineIssue = true;
                }

            foreach (var panel in ResponsePanels)
                if (panel is IDebateScanner scannable && scannable.Scan(out string panelLog))
                {
                    log += $"\n Issue on Response Panel index: {ResponsePanels.IndexOf(panel)} {panelLog}";

                    lineIssue = true;
                }

            return EndGroupDialogue == null || RetreatDialogue == null || lineIssue;
        }
    }
}