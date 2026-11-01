//Author: Happy30
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Dialogues/Dialogue", fileName = "New Dialogue", order = 1)]
    public class Dialogue : DialogueBased<Line>, IInstantAbleDialogue
    {
        [FormerlySerializedAs("UnlockPause")]
        public bool unlockPause = false;
        public bool UnlockPause { get => unlockPause; set => unlockPause = value; }

        [FormerlySerializedAs("UnlockPauseOption")]
        public bool unlockPauseOption = false;
        public bool UnlockPauseOption { get => unlockPauseOption; set => unlockPauseOption = value; }

        [FormerlySerializedAs("PauseOptionNum")]
        public int pauseOptionNum = 0;
        public int PauseOptionNum { get => pauseOptionNum; set => pauseOptionNum = value; }

        [FormerlySerializedAs("PauseOptionTo")]
        public bool pauseOptionTo = true;
        public bool PauseOptionTo { get => pauseOptionTo; set => pauseOptionTo = value; }

        public List<Choice> Choices = new();
        public StartInvestigation StartInvestigation = new();
        public EndEvent EndEvent = new();

        public void SetEndEvent(EndEvent EndEvent) => this.EndEvent = EndEvent;

        public bool IsInstant = false;

        public virtual bool GetInstant() => IsInstant;

        public virtual void SetInstant(bool IsInstant) => this.IsInstant = IsInstant;


        public bool HasChoiceDialogueAtIndex(int index)
        {
            if (Choices.Count == 0) return false;
            return Choices[index].NextDialogue != null;
        }

        public override bool IsNested(DialogueCore dia, List<DialogueCore> history)
        {
            if (Choices.Count > 0)
            {
                foreach (Choice choice in Choices)
                {
                    if (choice.NextDialogue != null)
                    {
                        if (history.Contains(choice.NextDialogue))
                            return false;

                        if (choice.NextDialogue == dia)
                            return true;
                        else
                        {
                            history.Add(choice.NextDialogue);

                            if (choice.NextDialogue.IsNested(dia, history))
                                return true;
                        }
                    }
                }
            }

            return base.IsNested(dia, history);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool ContainsCharacterNamed(string name)
        {
            foreach (var line in lines)
                if (line.Speaker != null && line.Speaker.FirstName == name)
                    return true;

            return false;
        }
    }
}