//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class DialogueComponentCollection<T> where T : IDialogueComponent<T>
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeField]
        public List<T> components = new();

        /// <summary>
        ///
        /// </summary>
        public string orderFieldName = "";

        /// <summary>
        ///
        /// </summary>
        public int ID;


        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="orderFieldName"></param>
        /// <param name="ID"></param>
        public virtual void Initialize(Transform transform, string orderFieldName, int ID)
        {
            this.orderFieldName = orderFieldName;

            this.ID = ID;

            components = transform.root.GetComponentsInChildren<MonoBehaviour>()
                .OfType<T>()
                .Where(component => component.ExclusiveHandlerID == ID)
                .OrderBy(component => component.Order)
                .ToList();
        }
    }
}