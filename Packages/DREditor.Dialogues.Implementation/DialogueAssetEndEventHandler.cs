//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Ending;
using DREditor.EventObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEndEventHandler) + ", " + nameof(IDialogueInteruptable) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class DialogueAssetEndEventHandler : DialogueEndEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> EndEventComponents = new();


        /// <inheritdoc/>
        public override IEnumerator EndDialogueEventOptions<T>(DialogueBase<T> currentDialogueCore, Action exitParentRoutine)
        {
            foreach (DialogueComponent component in EndEventComponents)
            {
                if (component == null)
                    continue;

                if (component is IDialogueEndEventRoutine endEventRoutine)
                {
                    bool stopEarly = false;

                    yield return endEventRoutine.EvaluateEndEvent(currentDialogueCore, () => stopEarly = true);

                    if (stopEarly)
                    {
                        exitParentRoutine?.Invoke();

                        yield break;
                    }
                }

                if (component is IDialogueEndEvent endEvent && endEvent.EvaluateEndEvent(currentDialogueCore, DialogueInterruptedInvoke))
                {
                    exitParentRoutine?.Invoke();

                    yield break;
                }
            }

            yield break;
        }
    }
}