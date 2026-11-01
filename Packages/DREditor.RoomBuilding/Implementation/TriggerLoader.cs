//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomStateHandlerBase<TriggerStateData, EventTrigger>) + 
        " for types <" + nameof(TriggerStateData) + ", " + nameof(EventTrigger) + ">" +
        "\n\n")]
    public class TriggerLoader : RoomStateHandlerBase<TriggerStateData, EventTrigger>
    {
        /// <inheritdoc/>
        public override List<EventTrigger> Save() => SaveData();

        /// <inheritdoc/>
        protected override void LoadAssetProcess(List<EventTrigger> baseDatas, List<EventTrigger> instanceDatas = null) 
            => Load(baseDatas, instanceDatas);

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceDatas"></param>
        /// <returns></returns>
        protected virtual List<EventTrigger> GetValidInstanceTriggers(List<EventTrigger> instanceDatas = null)
            => instanceDatas?.Where(trigger => trigger.GenericActuator != null).ToList();

        /// <inheritdoc/>
        public override void Unload(bool inEditor) => UnloadEvents();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<EventTrigger> SaveData()
        {
            List<EventTrigger> triggers = new();

            var actList = FindObjectsByType<MonoBehaviour>().OfType<IActuatorBehaviour>().ToList();

            for (int i = 0; i < actList.Count(); i++)
            {
                EventTrigger trigger = new();

                trigger.SetActuator((GenericActuator)actList[i].Save());

                ISubsequentBehaviour[] subsequentBehaviours = actList[i].GetSubsequents();

                for (int j = 0; j < subsequentBehaviours.Count(); j++)
                    trigger.GenericSubsequents.Add((GenericSubsequent)subsequentBehaviours[j].Save());

                triggers.Add(trigger);
            }

            return triggers;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Triggers"></param>
        /// <param name="InstanceTriggers"></param>
        public static void Load(List<EventTrigger> Triggers, List<EventTrigger> InstanceTriggers = null)
        {
            InitializeTriggers(Triggers, InstanceTriggers);

            for (int i = 0; i < Triggers.Count; i++)
            {
                EventTrigger currentTrigger = Triggers[i];

                CheckInstanceTriggers(i, InstanceTriggers);

                GameObject go = new(currentTrigger.GenericActuator.Type);

                PrepareLoadActuators(go, currentTrigger, i, InstanceTriggers);

                PrepareLoadSubsequents(go, currentTrigger, i, InstanceTriggers);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Triggers"></param>
        /// <param name="InstanceTriggers"></param>
        /// <remarks></remarks>
        static void InitializeTriggers(List<EventTrigger> Triggers, List<EventTrigger> InstanceTriggers = null)
        {
            if (InstanceTriggers != null && InstanceTriggers.Count > 0)
            {
                for (int i = 0; i < Triggers.Count; i++)
                {
                    try
                    {
                        EventTrigger baseTrigger = Triggers[i];

                        EventTrigger instanceTrigger = InstanceTriggers[i];

                        if (baseTrigger.GenericSubsequents.Count != instanceTrigger.GenericSubsequents.Count)
                        {
                            UnityEngine.Debug.Log("CALLED REVERSE");

                            InstanceTriggers.Reverse();

                            break;
                        }
                    }
                    catch (Exception exception)
                    {
                        UnityEngine.Debug
                            .LogWarning("MINOR ERROR: A BASE OR INSTANCE PIECE OF DATA COULDN'T BE FOUND on  " + i + "\n" +
                            exception.ToString());
                    }
                }

                UnityEngine.Debug.Log("INSTANCE Trigger is: " + InstanceTriggers.Count);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceIndex"></param>
        /// <param name="InstanceTriggers"></param>
        static void CheckInstanceTriggers(int instanceIndex, List<EventTrigger> InstanceTriggers = null)
        {
            if (InstanceTriggers != null)
            {
                try
                {
                    UnityEngine.Debug.Log(InstanceTriggers.Count);

                    UnityEngine.Debug.Log(InstanceTriggers[instanceIndex].ToString());

                    UnityEngine.Debug.Log(InstanceTriggers[instanceIndex].GenericActuator.Triggered);

                    UnityEngine.Debug.Log(InstanceTriggers[instanceIndex].GenericSubsequents.Count);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("MINOR NOTICE: Logging Triggers had an error on: " + instanceIndex + 
                        "\nLikely that this instance data is from previous game export's save data." + 
                        "\n\n" + ex.ToString());
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="currentTrigger"></param>
        /// <param name="baseIndex"></param>
        /// <param name="InstanceTriggers"></param>
        static void PrepareLoadActuators(GameObject go, EventTrigger currentTrigger,
            int baseIndex, List<EventTrigger> InstanceTriggers = null)
        {
            //Type type = Type.GetType(currentTrigger.GenericActuator.Type);

            // This code must eventually remove below code with above code in the rework after the 2025 rework 
            // To remove the System.Reflection usage. Since regular actuators will be removed and all that will be left are generic.
            // Potentially something to at least check and correct the sent type?

            bool hasValidInstanceData = InstanceTriggers != null && InstanceTriggers.Count > 0 && baseIndex < InstanceTriggers.Count;

            bool hasValidInstanceActuator = hasValidInstanceData && InstanceTriggers[baseIndex].GenericActuator != null;

            string summonType = hasValidInstanceActuator ?
                InstanceTriggers[baseIndex].GenericActuator.Type.Split('.')[^1] 
                : currentTrigger.GenericActuator.Type;

            var actuatorType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(type => type.Name == summonType);

            UnityEngine.Debug.Log("REPORT: Actuator Type to load: " + summonType);

            go.AddComponent(actuatorType);

            var actuatorBehaviours = go.GetComponents<MonoBehaviour>().OfType<IActuatorBehaviour>();

            foreach (IActuatorBehaviour actuatorBehaviour in actuatorBehaviours)
                if (hasValidInstanceData)
                {
                    if (InstanceTriggers[baseIndex].GenericActuator == null)
                    {
                        if (string.IsNullOrEmpty(InstanceTriggers[baseIndex].Actuator.Type))
                            continue;
//#if UNITY_EDITOR
                        TriggerConverter.ConvertActuator(InstanceTriggers[baseIndex], InstanceTriggers[baseIndex].Actuator);
//#endif
                    }

                    LoadActuatorInstance(actuatorBehaviour, baseIndex, InstanceTriggers);
                }
                else
                    actuatorBehaviour.Load(currentTrigger.GenericActuator.Clone());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actuatorBehaviour"></param>
        /// <param name="baseIndex"></param>
        /// <param name="InstanceTriggers"></param>
        static void LoadActuatorInstance(IActuatorBehaviour actuatorBehaviour, int baseIndex, List<EventTrigger> InstanceTriggers = null)
        {
            if (InstanceTriggers[baseIndex].GenericActuator != null)
                actuatorBehaviour.Load(InstanceTriggers[baseIndex].GenericActuator);
            else
                actuatorBehaviour.Load(InstanceTriggers[baseIndex].Actuator);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="currentTrigger"></param>
        /// <param name="baseIndex"></param>
        /// <param name="InstanceTriggers"></param>
        static void PrepareLoadSubsequents(GameObject go, EventTrigger currentTrigger,
            int baseIndex, List<EventTrigger> InstanceTriggers = null)
        {
            try
            {
                LoadSubsequents(go, currentTrigger, baseIndex, InstanceTriggers);
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogWarning(nameof(TriggerLoader) + ": Failed to load Subsequents");

                UnityEngine.Debug.LogException(exception);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="currentTrigger"></param>
        /// <param name="baseIndex"></param>
        /// <param name="InstanceTriggers"></param>
        static void LoadSubsequents(GameObject go, EventTrigger currentTrigger,
            int baseIndex, List<EventTrigger> InstanceTriggers = null)
        {
            for (int x = 0; x < currentTrigger.GenericSubsequents.Count; x++)
            {
                GenericSubsequent genericSubsequent = currentTrigger.GenericSubsequents[x];

                //Type type = Type.GetType(genericSubsequent.Type);

                // This code must eventually remove below code with above code in the rework after the 2025 rework 
                // To remove the System.Reflection usage.
                var subsequentType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(type => type.Name == genericSubsequent.Type);

                go.AddComponent(subsequentType);

                var subsequentBehaviours = go.GetComponents<ISubsequentBehaviour>().ToList();

                if (InstanceTriggers != null &&
                    InstanceTriggers.Count > 0 &&
                    baseIndex < InstanceTriggers.Count &&
                    InstanceTriggers[baseIndex].GenericSubsequents.Count != 0)
                    subsequentBehaviours[x].Load(((GenericSubsequent)genericSubsequent.Clone())
                            .MergeInstance(InstanceTriggers[baseIndex].GenericSubsequents[x]));
                else
                    subsequentBehaviours[x].Load(genericSubsequent.Clone());
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static void UnloadEvents()
        {
            MonoBehaviour[] actuators = FindObjectsByType<MonoBehaviour>()
                .Where(behaviour => behaviour is IActuatorBehaviour).ToArray();

            foreach (MonoBehaviour behaviour in actuators)
#if UNITY_EDITOR
                if (behaviour.gameObject != null)
                    DestroyImmediate(behaviour.gameObject);
#else
                Destroy(behaviour.gameObject);
#endif
        }
    }
}