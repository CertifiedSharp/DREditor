//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using UnityEngine;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class Spot
    {
        /// <summary>
        ///
        /// </summary>
        public string spotName;

        /// <summary>
        ///
        /// </summary>
        public Vector3 position;

        /// <summary>
        ///
        /// </summary>
        public Vector3 rotation;

        /// <summary>
        ///
        /// </summary>
        public Vector2 size;

        /// <summary>
        ///
        /// </summary>
        public Vector2 center;

        /// <summary>
        ///
        /// </summary>
        public bool isAnswer = false;

        /// <summary>
        ///
        /// </summary>
        public TrialDialogue wrongDialogue;


        /// <summary>
        ///
        /// </summary>
        /// <param name="spotName"></param>
        public virtual void SetSpotName(string spotName) => this.spotName = spotName;

        /// <summary>
        ///
        /// </summary>
        /// <param name="isAnswer"></param>
        public virtual void SetIsAnswer(bool isAnswer) => this.isAnswer = isAnswer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="wrongDialogue"></param>
        public virtual void SetWrongDialogue(TrialDialogue wrongDialogue) => this.wrongDialogue = wrongDialogue;

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="center"></param>
        public virtual void SetTransformProperties(
            Vector3 position,
            Vector3 rotation,
            Vector2 size,
            Vector2 center)
        {
            this.position = position;
            this.rotation = rotation;
            this.size = size;
            this.center = center;
        }
    }
}