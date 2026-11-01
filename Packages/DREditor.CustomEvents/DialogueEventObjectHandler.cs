//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Dialogues.Events;
using UnityEngine;

namespace DREditor.CustomEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CustomDialogueEventHandler) +
        "\n\n")]
    [CustomDialogueEvent("")]
    public class DialogueEventObjectHandler : CustomDialogueEventHandler
    {
        /// <inheritdoc/>
        protected override void StartListeningToEvents()
        {
            DialogueEventSystem.StartListening("ObjectToPosition", MoveObject);

            DialogueEventSystem.StartListening("SpawnObject", SpawnObject);
        }

        /// <inheritdoc/>
        protected override void StopListeningToEvents()
        {
            DialogueEventSystem.StopListening("ObjectToPosition", MoveObject);

            DialogueEventSystem.StopListening("SpawnObject", SpawnObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void MoveObject(object o = null)
        {
            ObjectToPosition tuple = (ObjectToPosition)o;

            GameObject gameObject = GameObject.Find(tuple.objectName);

            if (gameObject != null)
            {
                if (tuple.localPos)
                    gameObject.transform.localPosition = tuple.position;
                else
                    gameObject.transform.position = tuple.position;

                if (tuple.useRotation)
                    gameObject.transform.eulerAngles = tuple.rotation;
            }
            else
                UnityEngine.Debug.LogWarning($"ALERT: The game object named {tuple.objectName} could not be found!");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void SpawnObject(object o = null)
        {
            if (o is GameObjectCapsulate g)
                Instantiate(g.value);
        }
    }
}