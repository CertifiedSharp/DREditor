//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Talkables;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.Actors.Implementation
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
	[ClassHeader("")]
    public class WorldSpriteChanger : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debug = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float spriteChangeTime = 0.47f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected StringWithEvent blurCamCode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string spriteBlurCamCode = "SpriteChange";


        /// <summary>
        ///
        /// </summary>
        protected bool spriteChanged = false;

        /// <summary>
        ///
        /// </summary>
        protected bool activelyChanging = false;


        /// <summary>
        ///
        /// </summary>
        /// <param name="talkable"></param>
        /// <param name="expressionMaterial"></param>
        /// <param name="expressionSprite"></param>
        public virtual void WorldSpriteChange(Talkable talkable, Material expressionMaterial, Sprite expressionSprite)
        {
            Actor actor = talkable != null ? (Actor)talkable : null;

            if (WorldSpriteChangeDataInvalid(actor, expressionMaterial, expressionSprite))
                return;

            StartCoroutine(WorldChangeSprite(actor, expressionMaterial, expressionSprite));

            if (blurCamCode != null)
                blurCamCode.SetValue(spriteBlurCamCode);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="expressionMaterial"></param>
        /// <param name="expressionSprite"></param>
        /// <returns></returns>
        protected virtual bool WorldSpriteChangeDataInvalid(Actor actor, Material expressionMaterial, Sprite expressionSprite)
        {
            bool result = ActorIsInvalid(actor) ||
                          ExpressionMaterialIsInvalid(expressionMaterial) ||
                          FrontSpriteMatchesName(actor, expressionSprite.name);

            if (result)
                SetSpriteChangeComplete();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        protected virtual bool FrontSpriteMatchesName(SelectableObject actor, string spriteName)
        {
            var sprite = actor.transform.GetComponentsInChildren<SpriteRenderer>().First();

            bool matches = sprite.sprite.name == spriteName && !isLoadingFile.Value;

            if (matches && debug)
                UnityEngine.Debug.LogWarning($"ANALYSIS: {nameof(WorldSpriteChanger)}: Front Sprite matched attempted sprite change. " +
                    $"PROPOSAL: Set this lines sprite change to \"No Change\" on the dialogue asset.");

            return matches;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        protected virtual bool ActorIsInvalid(Actor actor)
        {
            bool result = actor == null;

            if (result && debug)
                UnityEngine.Debug.LogWarning($"REPORT: {nameof(WorldSpriteChanger)}: Actor was null during sprite change.");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="expressionMaterial"></param>
        /// <returns></returns>
        protected virtual bool ExpressionMaterialIsInvalid(Material expressionMaterial)
        {
            bool valid = expressionMaterial != null;

            if (!valid && debug)
                UnityEngine.Debug.LogWarning($"REPORT: {nameof(WorldSpriteChanger)}: World sprite change ignored — material was null.");

            return !valid;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="expressionMaterial"></param>
        /// <param name="expressionSprite"></param>
        protected virtual IEnumerator WorldChangeSprite(Actor actor, Material expressionMaterial, Sprite expressionSprite)
        {
            activelyChanging = true;

            if (actor.TryGetComponent<ExpressionChanger>(out var changer))
                if (inFastForwardMode.Value || isLoadingFile.Value)
                    changer.FastForwardExpressionChange(expressionMaterial, expressionSprite);
                else
                    yield return changer.ChangeExpressionProcess(expressionMaterial, expressionSprite);

            SetSpriteChangeComplete();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetSpriteChangeComplete()
        {
            spriteChanged = true;

            activelyChanging = false;

            if (blurCamCode != null)
                blurCamCode.SetValue(spriteBlurCamCode);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IEnumerator WaitForSpriteToFinishChanging()
        {
            if (inFastForwardMode.Value)
                yield return new WaitUntil(() => spriteChanged);
            else
                yield return new WaitForSeconds(spriteChangeTime);

            spriteChanged = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsActivelyChanging() => activelyChanging;
    }
}