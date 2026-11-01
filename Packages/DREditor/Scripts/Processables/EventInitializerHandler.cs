//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using UnityEngine;

namespace DREditor.Processables
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class EventInitializerHandler : MonoBehaviour
    {
        [Tooltip("")]
        [SerializeField] protected ScriptableObject database = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (database != null && database is not IEventInitializable initializable)
                UnityEngine.Debug.LogError("ERROR: Database must inherit the interface " + nameof(IEventInitializable) +
                    $" on game object {gameObject.name} in order for this script to be used!");

            if (database == null)
                UnityEngine.Debug.LogWarning($"ALERT: There is no initializable object assigned on game object {gameObject.name}!");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => ((IEventInitializable)database)?.Initialize();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy() => ((IEventInitializable)database)?.Resolve();
    }
}