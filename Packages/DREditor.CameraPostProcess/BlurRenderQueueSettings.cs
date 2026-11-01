//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.CamerasPostProcess
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Game Settings/Blur Render Queue Settings", fileName = "BlurRenderQueueSettings")]
    public class BlurRenderQueueSettings : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public bool useRenderQueue = false;


        public virtual void PopulateData(ScriptableObject obj)
        {
            if (obj is BlurRenderQueueSettings settings)
                useRenderQueue = settings.useRenderQueue;
        }
    }
}