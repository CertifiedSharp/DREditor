//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;

namespace DREditor.Editor.AssetTextEditing
{
    /// <summary>
    ///
    /// </summary>
    public static class AssetEditorUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateProcess"></param>
        public static void UpdateAllAssetsOfType<T>(Action<T> updateProcess) where T : UnityEngine.Object
        {
            List<AssetEditProcess<T>> processes = new();

            var assets = BuilderUtility.FindAssetsByType<T>();

            foreach (var asset in assets)
                processes.Add(new AssetEditProcess<T>
                {
                    Target = asset,
                    Action = updateProcess
                });

            new AssetEditor<T>(processes).ApplyAssetEditing();
        }
    }
}