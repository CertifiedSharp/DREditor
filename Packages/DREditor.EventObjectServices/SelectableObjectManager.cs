//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<IObjectPooler<SelectableObject>>) + " for type: " + nameof(IObjectPooler<SelectableObject>) +
        " of type: " + nameof(SelectableObject) +
        "\n\n")]
    public class SelectableObjectManager : ServiceHolder<IObjectPooler<SelectableObject>>, IObjectPooler<SelectableObject>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<SelectableObject> selectables = new();


        /// <inheritdoc/>
        public virtual List<SelectableObject> GetRegisteredObjects() => selectables;

        /// <inheritdoc/>
        public virtual void RegisterObject(SelectableObject obj) => selectables.Add(obj);

        /// <inheritdoc/>
        public virtual void UnregisterObject(SelectableObject obj) => selectables.Remove(obj);

        /// <inheritdoc/>
        public virtual SelectableObject GetObjectByName(string name)
        {
            var matches = GetRegisteredObjects().Where(talkable => talkable.gameObject.name == name);

            if (matches.Any())
                return matches.First();

            return null;
        }
    }
}