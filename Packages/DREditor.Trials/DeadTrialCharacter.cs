//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System;
using System.Linq;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Death/Dead Trial Character", fileName = "DeadTrialCharacter")]
    [Serializable]
    public class DeadTrialCharacter : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public string characterName;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public GameObject prefab;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector3 position;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector3 eulerAngles;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool usePrefabTransform = false;


        /// <summary>
        ///
        /// </summary>
        public virtual void SetUpDeadCharacterInTrial()
        {
            GameObject portrait = Instantiate(prefab);

            if (!usePrefabTransform)
            {
                portrait.transform.position = position;

                portrait.transform.eulerAngles = eulerAngles;
            }
            
            Transform characterTransform = GetCharacter(characterName);

            if (characterTransform != null)
                characterTransform.position = new Vector3(0, -100, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual Transform GetCharacter(string name)
        {
            var actors = FindObjectsByType<SelectableObject>();

            var foundActors = actors.Where(n => n.gameObject.transform.name == name);

            if (foundActors.Count() > 0)
                return foundActors.First().transform;

            UnityEngine.Debug.LogWarning(nameof(DeadTrialCharacter) + ": WARNING: Could not find character for: " + name);

            return null;
        }
    }
}