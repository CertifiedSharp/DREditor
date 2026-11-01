//Author: Happy30
using DREditor.Processables;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.EventObjects
{
    public class SceneEventListener : MonoBehaviour, ISceneEventListener, IPrioritizable
    {
        public SceneEvent Event;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public BoolWithEvent Condition;

        /// <summary>
        /// 
        /// </summary>
        [ShowIf(nameof(ConditionAssigned))]
        [Tooltip("")]
        [SerializeField] protected bool conditionMustBe = true;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int priority = 0;


        public virtual int Priority { get => priority; set => priority = value; }


        public UnityEvent Response;

        
        protected virtual void OnEnable()
        {
            if (Event != null)
                Event.RegisterListener(this);
        }

        protected virtual void OnDisable()
        {
            if (Event != null)
                Event.UnregisterListener(this);
        }

        public virtual void OnEventRaised()
        {
            if (ConditionAssigned() && Condition.Value != conditionMustBe)
                return;

            OnResponseInvoked();
        }

        public virtual void AddListener(UnityAction action) => Response.AddListener(action);

        public virtual void RemoveListener(UnityAction action) => Response.RemoveListener(action);


        protected virtual void OnResponseInvoked() => Response.Invoke();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ConditionAssigned() => Condition != null;
    }
}