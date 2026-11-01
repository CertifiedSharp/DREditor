//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ITweenWaiter) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class ActorSpriteFader : MonoBehaviour, ITweenWaiter, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent spriteIsFading = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [Range(0f, 1f)]
        [SerializeField] protected float appearToValue = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [Range(0f, 1f)]
        [SerializeField] protected float leaveToValue = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float actorFadingBufferTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeDurationTime = 1f;


        /// <summary>
        ///
        /// </summary>
        protected IDialogueFinisher DialogueFinisher = null;

        /// <summary>
        ///
        /// </summary>
        protected readonly List<Actor> fadedActors = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            DialogueFinisher = ServiceLocator.GetService<IDialogueManager>().GetFinisher();

            DialogueFinisher.SubscribeToOnFinishOfDialogue(OnFinishedDialogue);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => DialogueFinisher.UnsubscribeToOnFinishOfDialogue(OnFinishedDialogue);


        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="appearToOverride"></param>
        public virtual void ActorAppear(Actor actor, float appearToOverride = -1)
        {
            float appearVisibilityValue = appearToOverride > 0 ? appearToOverride : appearToValue;

            StartCoroutine(Fading(actor, appearVisibilityValue));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="leaveToOverride"></param>
        public virtual void ActorDisappear(Actor actor, float leaveToOverride = -1)
        {
            fadedActors.Add(actor);

            UnityEngine.Debug.Log($"{ToString()}: Fading Actor Event: " + actor.gameObject.name);

            float leaveVisibilityValue = leaveToOverride > 0 ? leaveToOverride : leaveToValue;

            StartCoroutine(Fading(actor, leaveVisibilityValue));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public virtual IEnumerator Fading(Actor actor, float to)
        {
            if (isLoadingFile.Value)
            {
#if DOTWEEN
                actor.GetComponentInChildren<SpriteRenderer>().DOFade(to, 0);
#endif
                yield break;
            }

            spriteIsFading.SetValue(true);

            yield return new WaitForSeconds(actorFadingBufferTime);
#if DOTWEEN
            actor.GetComponentInChildren<SpriteRenderer>().DOFade(to, fadeDurationTime);
#endif
            yield return new WaitForSeconds(fadeDurationTime);

            spriteIsFading.SetValue(false);
        }


        /// <inheritdoc/>
        public virtual IEnumerator WaitForTweenToFinish()
        {
            yield return new WaitUntil(() => !spriteIsFading.Value);

            yield break;
        }

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => ClearFadedActors();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearFadedActors() => fadedActors.Clear();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnFinishedDialogue()
        {
            SetAllFadedActorSpritesVisible();

            ClearFadedActors();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetAllFadedActorSpritesVisible()
        {
            for (int i = 0; i < fadedActors.Count; i++)
                if (fadedActors[i] != null &&
                    fadedActors[i].GetComponentInChildren<SpriteRenderer>() is SpriteRenderer renderer &&
                    renderer.color.a == 0)
#if DOTWEEN
                    renderer.DOFade(1, 0);
#else
                    renderer.color = new(renderer.color.r, renderer.color.g, renderer.color.g, 1);
#endif
        }
    }
}