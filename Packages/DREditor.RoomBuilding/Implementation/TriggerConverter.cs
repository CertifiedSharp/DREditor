//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Toolbox;
#endif
using DREditor.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace DREditor.RoomBuilding.Implementation
{
    /* 
     * Conversion Process
     * - Create all the new subsequent class types.
     * - Get all the class types inheriting from GenericSubsequent like how we do IDialogueEvent shit.
     * - Create a new instance of them. 
     * - Once created The GenericSubsequent class should have an abstract function bool CanConvert that passes a subsequent in the 
     *      Event Trigger's Subsequents where it returns true if it can convert.
     * - If it can convert it then call an abstract function Convert for that class.
     * - Since it will return an actual instance of the specific type of subsequent,
     *      it can be added to the event trigger's genericsubsequent list.
     * 
     * Update Saving Loading Process
     * - The loading process should now use the genericsubsequent list instead of the subsequent list.
     * - For Saving, the save function can instead pass the generic subsequent list where on the monobehaviour for the subsequent
     *      you can add the specific subsequent type.
     */

    /// <summary>
    ///
    /// </summary>
    public static class TriggerConverter
    {
        /// <summary>
        ///
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("DREditor/Convert/Triggers/Convert All Trigger Assets")]

        public static void ConvertAllTriggerAssets()
        {
            var triggerAssets = VerifyDialogues.FindAssetsByType<StateDataAsset<EventTrigger>>();

            foreach (var trigger in triggerAssets)
                ConvertTriggerAsset(trigger);
        }
#endif

        /// <summary>
        ///
        /// </summary>
        /// <param name="trigger"></param>
        public static void ConvertTriggerAsset(StateDataAsset<EventTrigger> trigger)
        {
            foreach (var data in trigger.datas)
                ConvertEventTrigger(data);
#if UNITY_EDITOR
            EditorUtility.SetDirty(trigger);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trigger"></param>
        static void ConvertEventTrigger(EventTrigger trigger)
        {
            ConvertActuator(trigger, trigger.Actuator);

            trigger.GenericSubsequents.Clear();

            foreach (var subsequent in trigger.Subsequents)
                ConvertSubsequent(trigger, subsequent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="actuator"></param>
        public static void ConvertActuator(EventTrigger trigger, Actuator actuator)
        {
            var genericActuatorClasses = GetInheritedClasses<GenericActuator>();

            foreach (var genericClass in genericActuatorClasses)
            {
                var convertable = (IActuatorConvert)Activator.CreateInstance(genericClass);

                if (convertable.CanConvert(actuator))
                {
                    convertable.Convert(ref trigger.GenericActuator, actuator);

                    return;
                }
            }

            UnityEngine.Debug.LogWarning("COULD NOT CONVERT ACTUATOR FOR: " + actuator.Type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="subsequent"></param>
        static void ConvertSubsequent(EventTrigger trigger, Subsequent subsequent)
        {
            var genericSubsequentClasses = GetInheritedClasses<GenericSubsequent>();

            foreach (var genericClass in genericSubsequentClasses)
            {
                var convertable = (ISubsequentConvert)Activator.CreateInstance(genericClass);

                if (convertable.CanConvert(subsequent))
                {
                    convertable.Convert(trigger.GenericSubsequents, subsequent);

                    return;
                }
            }

            UnityEngine.Debug.LogWarning("COULD NOT CONVERT SUBSEQUENT FOR: " + subsequent.Type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static List<Type> GetInheritedClasses<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToList();
        }
    }
}