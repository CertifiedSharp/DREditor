//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Services;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DestroyFlairHandler) +
        "\n\n")]
    public class DestroyFlairOnNextLine : DestroyFlairHandler
    {
        /// <summary>
        ///
        /// </summary>
        protected ILineReader lineReader;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            InitializeCheck();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeCheck()
        {
            Initialize();

            if (InitializationFailed())
                OnInitializationCheckFailed();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize() => 
            lineReader = ServiceLocator.GetService<IDialogueManager>().GetReader().GetComponentInChildren<ILineReader>();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitializationFailed() => lineReader == null;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnInitializationCheckFailed()
        {
            UnityEngine.Debug.LogError($"ALERT: Destroyer {gameObject.name} failed initialization because the line reader was null!" +
                "\nDestroying Object.");

            InvokeFlairDestroyer();
        }

        protected virtual void OnEnable()
        {
            if (lineReader != null)
                lineReader.OnLineEndEvent += AttemptToDestroyFlair;
        }

        protected virtual void OnDisable()
        {
            if (lineReader != null)
                lineReader.OnLineEndEvent -= AttemptToDestroyFlair;
        }

        /// <inheritdoc/>
        protected override void OnOtherHandlerDestroying() { }
    }
}