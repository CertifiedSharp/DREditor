//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectPooler<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        void RegisterObject(T obj);

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetObjectByName(string name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        void UnregisterObject(T obj);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        List<T> GetRegisteredObjects();
    }
}