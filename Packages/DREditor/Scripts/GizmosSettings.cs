//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// Project settings relating to gizmos drawn within DREditor.
    /// </summary>
    public class GizmosSettings : ProjectSettingsData<GizmosSettings>
    {
        /// <summary>
        /// The default radius to draw gizmo spheres.
        /// </summary>
        [Tooltip("The default radius to draw gizmo spheres.")]
        public float gizmoSphereRadius = 1;
    }
}