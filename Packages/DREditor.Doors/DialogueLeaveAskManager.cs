//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using UnityEngine;

namespace DREditor.Doors
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(LeaveAskManager) + 
        "\n\n")]
    public class DialogueLeaveAskManager : LeaveAskManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueCore leaveAsk = null;


        /// <inheritdoc/>
        public override DialogueCore GetLeaveAskDialogue()
        {
            if (leaveAsk == null)
                UnityEngine.Debug.LogWarning("Leave Ask has not been added! please add it to the progression manager");

            return leaveAsk;
        }
    }
}