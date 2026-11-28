//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.Gates;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// Attribute that displays a drop down of all types that inherits the field type so the user can choose which type to use.
    /// </summary>
    /// <remarks>
    /// It should be noted that the reason this attribute is used given that the TypeList class exists
    /// is because TypeList is focused on the same functionality as this attribute except applicable to custom editors only. <br/>
    /// Where as this attribute can be applied for scripts without custom editors only.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SelectTypeAttribute : PropertyAttribute
    {
        /// <summary>
        /// Text to display for the drop down's tool tip.
        /// </summary>
        public string toolTipText;


        /// <summary>
        /// Attribute that displays a drop down of all types that inherits the field type so the user can choose which type to use.
        /// </summary>
        /// <param name="toolTipText">Text to display for the drop down's tool tip.</param>
        /// <remarks>
        /// It should be noted that the reason this attribute is used given that the TypeList class exists
        /// is because TypeList is focused on the same functionality as this attribute except applicable to custom editors only. <br/>
        /// Where as this attribute can be applied for scripts without custom editors only.
        /// </remarks>
        public SelectTypeAttribute(string toolTipText = "") => this.toolTipText = toolTipText;
    }

    public interface IDynamicCollection<T> : IDynamicCollection
    {
        new List<T> DynamicDatas { get; }
    }
    public interface IDynamicCollection
    {
        IEnumerable<object> DynamicDatas { get; }
    }
    public static class DynamicCollectionExtensions
    {
        public static T Get<T>(this IDynamicCollection collection)
        {
            foreach (var data in collection.DynamicDatas)
                if (data is T soughtData)
                    return soughtData;

            return default;
        }

        public static List<T> GetAll<T>(this IDynamicCollection collection)
        {
            List<T> result = new();

            foreach (var data in collection.DynamicDatas)
                if (data is T soughtData)
                    result.Add(soughtData);

            return result;
        }

        public static bool Has<T>(this IDynamicCollection collection, out T soughtData)
        {
            soughtData = default;

            foreach (var data in collection.DynamicDatas)
                if (data is T foundData)
                    soughtData = foundData;

            return soughtData is not null;
        }

        public static bool HasMany<T>(this IDynamicCollection collection, out List<T> soughtDatas)
        {
            soughtDatas = new();

            foreach (var data in collection.DynamicDatas)
                if (data is T foundData)
                    soughtDatas.Add(foundData);

            return soughtDatas.Count != 0;
        }
    }
}