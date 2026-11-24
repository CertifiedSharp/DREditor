//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DialogueBase<T> : DialogueCore, IEndEventCollection
    {
        /// <summary>
        /// 
        /// </summary>
        [FormerlySerializedAs("Lines")]
        public List<T> lines = new();

        /// <summary>
        /// 
        /// </summary>
        public List<T> Lines { get => lines; }

        /// <summary>
        /// The Collection of <see cref="IEndEvent"/>s held by the implementer.
        /// </summary>
        [SerializeReference]
        protected List<IEndEvent> endEvents = new();

        /// <inheritdoc/>
        public virtual List<IEndEvent> EndEvents { get => endEvents; }

        /// <inheritdoc/>
        public override int LineCount => Lines.Count;

        /// <inheritdoc/>
        public override void InvokeDialogue(ITypeCallback callback) { }


        public SceneTransition SceneTransition = new();

        public TriggerFlag FlagTrigger = new();

        public Variable Variable = new();

        public DirectTo DirectTo;


        /// <inheritdoc/>
        public virtual T2 GetEndEvent<T2>()
        {
            foreach (var endEvent in EndEvents)
                if (endEvent is T2 result)
                    return result;

            return default;
        }

        /// <inheritdoc/>
        public override Line GetLine(int index)
        {
            int lineIndex = 0;

            if (index < Lines.Count && index >= 0)
                lineIndex = index;
            else
                UnityEngine.Debug.LogError($"ALERT: The line at the requested index ({index}) is out of bounds, defaulting to 0.");

            if (Lines[lineIndex] is Line castedLine)
                return castedLine;
            else
            {
                UnityEngine.Debug.LogWarning("ALERT: The line retrieved within the dialogue asset was not of type " + nameof(Line));

                return null;
            }
        }

        /// <inheritdoc/>
        public override List<T2> GetLines<T2>()
        {
            if (Lines is List<T2> castedLines)
                return castedLines;

            if (Lines != null && typeof(T2).IsAssignableFrom(typeof(T)))
                return Lines.Cast<T2>().ToList();

            return null;
        }

        /// <inheritdoc/>
        public override bool IsNested(DialogueCore dia, List<DialogueCore> history)
        {
            if (GetEndEvent<DirectTo>() is DirectTo directTo && directTo.NewDialogue != null)
            {
                if (history.Contains(directTo.NewDialogue))
                    return false;

                if (directTo.NewDialogue == dia)
                    return true;
                else
                {
                    history.Add(this);

                    return directTo.NewDialogue.IsNested(dia, history);
                }
            }

            return false;
        }


#if UNITY_EDITOR
        /* ALERT: The following should be applied to this script after DREditor Ver.0.1.12 :
         * Remove all this code. By this point End Events should now be done converting for projects.
        */

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] protected bool converted = false;

        /// <summary>
        /// 
        /// </summary>
        protected void SetConverted() => converted = true;

        /// <inheritdoc/>
        public override void ConvertEndEvents()
        {
            if (converted)
                return;

            ConvertEndEventInstances();

            ClearOldEndEvents();

            SetConverted();

            UnityEditor.EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ConvertEndEventInstances()
        {
            AddExistingEvent(SceneTransition);

            AddExistingEvent(FlagTrigger);

            AddExistingEvent(DirectTo);

            AddExistingEvent(Variable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endEvent"></param>
        protected virtual void AddExistingEvent(EndEventBase endEvent)
        {
            if (endEvent != null && endEvent.Enabled)
                EndEvents.Add(endEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ClearOldEndEvents()
        {
            SceneTransition = null;

            FlagTrigger = null;

            DirectTo = null;

            Variable = null;
        }
#endif
    }
}