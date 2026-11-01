//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.LocalDialogues;
using DREditor.Services;
using DREditor.Talkables;

namespace DREditor.Actors
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(Talkable) +
        "\n\n")]
    public class Actor : Talkable
    {
        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            if (LocalDialogueHandlerBase.BookOfTriggers.ContainsKey(gameObject.name))
                LocalDialogueHandlerBase.BookOfTriggers.Remove(gameObject.name);

            LocalDialogueHandlerBase.BookOfTriggers.Add(gameObject.name, this);
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            LocalDialogueHandlerBase.BookOfTriggers.Remove(gameObject.name);

            base.OnDestroy();
        }

        /// <inheritdoc/>
        public override void SelectObject()
        {
            Dialogue[] conversation = new Dialogue[1];

            conversation[0] = ServiceLocator.GetService<LocalDialogueHandlerBase>().EvaluateDialogue(conversations);

            if (conversation[0] != null && conversation[0].ContainsCharacterNamed(gameObject.name) && !identified)
                identified = true;

            InvokeConversation(conversation);
        }

        /// <inheritdoc/>
        public override void SkipConversation(int conversationIndex)
        {
            base.SkipConversation(conversationIndex);

            if (!identified)
                identified = true;
        }

        /// <inheritdoc/>
        public override string DisplayName
        {
            get => base.DisplayName;

            set
            {
                if (displayName == unidentifiedDisplayName)
                    return;
                else
                    base.DisplayName = value;
            }
        }

        public virtual void SetIdentified(bool identified) => this.identified = identified;

        public virtual bool GetIdentified() => identified;
    }
}