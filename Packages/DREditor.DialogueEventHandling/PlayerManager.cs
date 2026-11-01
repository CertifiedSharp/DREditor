//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.FPC;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace DREditor.DialogueEventHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    [DefaultExecutionOrder(-1000)]
    public class PlayerManager : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent controllersEnabled = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Volume blur = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent[] cameras;


        /// <summary>
        ///
        /// </summary>
        protected bool isMaster = false;

        /// <summary>
        ///
        /// </summary>
        protected ControlMonobehaviours[] controllers = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            var managers = FindObjectsByType<PlayerManager>();

            if (managers.Count() == 1 && managers[0] == this)
                isMaster = true;
            else if (managers.Count() > 1 && !isMaster)
            {
                if (GetComponentInChildren<EventSystem>() is EventSystem eventSystem)
                    Destroy(eventSystem.gameObject);

                Destroy(gameObject);
            }

            controllers = FindObjectsByType<ControlMonobehaviours>();

            Initialize();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            DontDestroyOnLoad(gameObject);

            if (inTrialMode != null && inTrialMode.Value)
                DisablePlayer();
        }


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            Response.AddListener(ResetControllers);

            DialogueEventSystem.StartListening("DisablePlayer", DisablePlayer);

            DialogueEventSystem.StartListening("DisableAbility", DisableAbility);

            DialogueEventSystem.StartListening("DisableControlScripts", DisableControlScripts);

            DialogueEventSystem.StartListening("EnableControlMonobehaviour", EnableMonoBehaviourControllers);

            DialogueEventSystem.StartListening("DisableControlMonobehaviour", DisableMonoBehaviourControllers);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            Response.RemoveListener(ResetControllers);

            DialogueEventSystem.StopListening("DisablePlayer", DisablePlayer);

            DialogueEventSystem.StopListening("DisableAbility", DisableAbility);

            DialogueEventSystem.StopListening("DisableControlScripts", DisableControlScripts);

            DialogueEventSystem.StopListening("EnableControlMonobehaviour", EnableMonoBehaviourControllers);

            DialogueEventSystem.StopListening("DisableControlMonobehaviour", DisableMonoBehaviourControllers);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void DisablePlayer(object o = null)
        {
            foreach (var camera in cameras)
                camera.SetValue(false);

            DisableAbility();

            blur.enabled = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void DisableAbility(object o = null)
        {
            DisableControlScripts();

            foreach (var controller in controllers)
                controller.EnableTPFD(false);

            DisableMonoBehaviourControllers();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void DisableControlScripts(object o = null)
        {
            foreach (var controller in controllers)
                controller.EnableBehaviours(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void DisableMonoBehaviourControllers(object o = null) => SetMonoBehaviourControllersTo(false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void EnableMonoBehaviourControllers(object o = null) => SetMonoBehaviourControllersTo(true);

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        protected virtual void SetMonoBehaviourControllersTo(bool to)
        {
            foreach (var controller in controllers)
            {
                controller.enabled = to;

                controller.SetSetting(to);
            }

            controllersEnabled.SetValue(to);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetControllers()
        {
            DisableMonoBehaviourControllers();

            foreach (var controller in controllers)
            {
                controller.SetDisable(false);

                controller.SetSetting(false);
            }
        }
    }
}