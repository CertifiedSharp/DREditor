//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Progressing.Handling;
using DREditor.Services;
using UnityEngine;

namespace DREditor.Spawnables.Implementation
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class SpawnCondition : Spawnable, ISaveable, ISceneEventListener
    {
        public int SubID { get => subID; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        [SerializeField] protected int subID = 0;

        /// <summary>
        ///
        /// </summary>
        public VariableCapsule InstanceData = new();

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public SceneEvent sceneEvent = null;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            sceneEvent.RegisterListener(this);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            sceneEvent.UnregisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        /// <param name="sceneEvent"></param>
        public virtual void SetSceneEvent(SceneEvent sceneEvent) => this.sceneEvent = sceneEvent;

        /// <summary>
        ///
        /// </summary>
        /// <param name="subID"></param>
        public virtual void SetSubID(int subID) => this.subID = subID;

        /// <summary>
        ///
        /// </summary>
        public virtual void OnEventRaised()
        {
            if (InstanceData.ignoreCondition)
                UnityEngine.Debug.Log($"{gameObject.name}: Events Conditions for invoking have been set to be ignored.");
            else if (AnyConditionsFailed())
                return;

            OnConditionPassed();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AnyConditionsFailed()
        {
            foreach (var variable in InstanceData.Conditions)
                if (SpawnConditionFailed(variable))
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        protected virtual bool SpawnConditionFailed(VariableBase variable)
        {
            bool failed = !ServiceLocator.GetService<FlagTriggerHandlerBase>()
                .EvaluateVariable(variable.Chapter, variable.Objective, variable.Flags, variable.FalseFlags);

            if (failed)
                UnityEngine.Debug.LogWarning($"{gameObject.name}: The Spawn Condition evaluated Failed.");

            return failed;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnConditionPassed()
        {
            UnityEngine.Debug.Log($"{gameObject.name}: The Spawn Condition Variable Passed");

            foreach (IDialogueEvent e in InstanceData.DiaEvents)
                e.TriggerDialogueEvent();
        }


        /// <inheritdoc/>
        public virtual string Save() => JsonUtility.ToJson(InstanceData, true);

        /// <inheritdoc/>
        public virtual void Load(string data)
        {
            InstanceData = JsonUtility.FromJson<VariableCapsule>(data);

            if (Application.isPlaying && InstanceData != null && InstanceData.ignoreCondition)
                OnEventRaised();
        }
    }
}