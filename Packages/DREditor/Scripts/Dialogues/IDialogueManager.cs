//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public interface IDialogueManager : ITypeCallback
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="DebugNoDialogue"></param>
        void DebugNoCharacterDialogueCall(SelectableObject actor, DialogueCore DebugNoDialogue = null);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actor"></param>
        /// <param name="dialogues"></param>
        /// <param name="inleaving"></param>
        void StartDialogue<T>(SelectableObject actor, DialogueBase<T>[] dialogues, bool inleaving = false) where T : Line;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actor"></param>
        /// <param name="dialogue"></param>
        /// <param name="inleaving"></param>
        void StartDialogue<T>(SelectableObject actor, DialogueBase<T> dialogue, bool inleaving = false) where T : Line;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        GameObject GetManagerObject();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DialogueReader GetReader();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IDialogueEnder GetEnder();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IDialogueFinisher GetFinisher();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetManagerComponent<T>() where T : MonoBehaviour;


        void StopAllCoroutines();

        T[] GetComponentsInChildren<T>();
    }
}