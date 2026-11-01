//Author: Benjamin "Sweden" Jillson : sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Dialogues/Multi Trial Dialogue", fileName = "New MultiTrial Dialogue")]
    public class MultiTrialDialogue : DialogueBased<MultiTrialLine>, ITrialSequenceSkippable
    {
#if UNITY_EDITOR
        public bool autoAssignPanelNums = true;
        public List<int> panelCharacterNumbers = new();
        public List<int> assignedPanelNumbers = new();
#endif

        public int skip = 0;

        public int Skip { get => skip; set => skip = value; }

#if UNITY_EDITOR
        public void SetAutoAssign(bool autoAssignPanelNums) => this.autoAssignPanelNums = autoAssignPanelNums;
#endif
    }

    [System.Serializable]
    public class MultiTrialLine : TrialLine, IMultiPanelHolder
    {
        public bool showEditorPanels = false;
        public int panelNum = 0;
        public List<MultiPanel> multiPanels = new();

        public MultiTrialLine() { }

        public MultiTrialLine(List<MultiPanel> multiPanels) => this.multiPanels = multiPanels;

        public bool ShowEditorPanels { get => showEditorPanels; set => showEditorPanels = value; }

        public int PanelNumber { get => panelNum; set => panelNum = value; }

        public virtual List<MultiPanel> GetMultiPanels() => multiPanels;
    }

    public interface IMultiPanelHolder
    {
        /// <summary>
        ///
        /// </summary>
        bool ShowEditorPanels { get; set; }

        /// <summary>
        ///
        /// </summary>
        int PanelNumber { get; set; }

        List<MultiPanel> GetMultiPanels();
    }

    [System.Serializable]
    public class MultiPanel : LineBase, ITrialLine
    {
        public int panelNum;

        public int camAnimIdx;

        public TCO co = null;


        public void SetPanelNum(int panelNum) => this.panelNum = panelNum;


        public int CamAnimIdx { get => camAnimIdx; set => camAnimIdx = value; }

        public TCO Co { get => co; set => co = value; }


        public virtual TrialLine ConvertToLine()
        {
            TrialLine line = new(Speaker, SpeakerNumber, Expression, ExpressionNumber)
            {
                CamAnimIdx = camAnimIdx,

                Co = co
            };

            return line;
        }

        public virtual void Merge(TrialLine line)
        {
            SetSpeaker(line.Speaker);

            SetSpeakerNum(line.SpeakerNumber);

            SetExpression(line.Expression);

            SetExpressionNum(line.ExpressionNumber);

            CamAnimIdx = line.CamAnimIdx;

            Co = line.Co;
        }
    }
}