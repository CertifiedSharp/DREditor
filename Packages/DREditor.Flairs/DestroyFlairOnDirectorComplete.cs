//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
//Original Author: KHeartz
using UnityEngine;
using UnityEngine.Playables;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DestroyFlairHandler) +
        "\n\n")]
    public class DestroyFlairOnDirectorComplete : DestroyFlairHandler
    {
        /// <summary>
        ///
        /// </summary>
        protected PlayableDirector timeline = null;

        /// <summary>
        ///
        /// </summary>
        protected bool isLooping = false;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            if (TryGetComponent(out timeline))
                isLooping = timeline.extrapolationMode == DirectorWrapMode.Loop;
        }

        protected virtual void Update() => AttemptToDestroyFlair();

        /// <inheritdoc/>
        protected override bool FlairCanBeDestroyed() => base.FlairCanBeDestroyed() && IsComplete();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsComplete() => !timeline.playableGraph.IsValid() || !isLooping && !timeline.playableGraph.IsPlaying();


        /// <inheritdoc/>
        protected override void InvokeFlairDestroyer()
        {
            DestroyPlayableDirector();

            base.InvokeFlairDestroyer();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DestroyPlayableDirector()
        {
            if (gameObject.TryGetComponent(out PlayableDirector playableDirector))
            {
                playableDirector.enabled = false;

                foreach (var item in gameObject.GetComponentsInChildren<Animator>())
                    Destroy(item);

                Destroy(gameObject.GetComponent<Animator>());
            }
        }

        /// <inheritdoc/>
        protected override void OnOtherHandlerDestroying() => DestroyPlayableDirector();
    }
}