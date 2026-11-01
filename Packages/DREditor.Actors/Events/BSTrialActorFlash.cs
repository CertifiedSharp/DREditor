//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Cameras;
using DREditor.Characters;
using DREditor.Services;
using DREditor.Talkables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class BSTrialActorFlash : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("mat")]
        [SerializeField] protected Material material = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string shaderColorID = "outlineColor";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float animTime = .125f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float alphaTo = .5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int loops = 4;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LoopType loopType = LoopType.Yoyo;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] List<Color> colors = new();


        /// <summary>
        ///
        /// </summary>
        protected DRTrialCamera trialCamera = null;

        /// <summary>
        ///
        /// </summary>
        protected int seatPos = 0;

        /// <summary>
        ///
        /// </summary>
        protected Material originalMaterial = null;

        /// <summary>
        ///
        /// </summary>
        protected Color defaultColor;

        /// <summary>
        ///
        /// </summary>
        protected float currentAlpha = 0;

        /// <summary>
        ///
        /// </summary>
        protected bool animating = false;

        /// <summary>
        ///
        /// </summary>
        protected Actor actor;

        /// <summary>
        ///
        /// </summary>
        protected MeshRenderer characterMesh = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => trialCamera = FindAnyObjectByType<DRTrialCamera>();

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void Flash()
        {
            Talkable talkable = GetTalkable();

            if (talkable == null)
                return;

            actor = talkable as Actor;

            StartCoroutine(FlashOnCharacter());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Talkable GetTalkable()
        {
            seatPos = Mathf.RoundToInt(trialCamera.SeatFocus);

            foreach (var character in characterDatabase.Characters)
                if (character.TrialPosition == seatPos)
                    return ServiceLocator.GetService<IObjectPooler<Talkable>>().GetObjectByName(character.FirstName);

            UnityEngine.Debug.LogError($"{ToString()}: Could not find character for seat position {seatPos}");

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator FlashOnCharacter()
        {
            PrepareAnimation();

            yield return AnimateFlash();

            ResolveAnimation();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PrepareAnimation()
        {
            PrepareMaterial();

            SetMaterialColor();

            animating = true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PrepareMaterial()
        {
            characterMesh = actor.GetComponentInChildren<MeshRenderer>();

            originalMaterial = characterMesh.material;

            defaultColor = material.GetColor(shaderColorID);

            material.mainTexture = originalMaterial.mainTexture;

            characterMesh.material = material;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetMaterialColor()
        {
            if (seatPos < colors.Count)
            {
                Color color = colors[seatPos];

                characterMesh.material.SetColor(shaderColorID, new Color(color.r, color.g, color.b, 0));
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator AnimateFlash()
        {
            StartCoroutine(Setter());
#if DOTWEEN
            DOTween.To(() => currentAlpha, y => currentAlpha = y, alphaTo, animTime)
                .SetLoops(loops, loopType);
#endif
            yield return new WaitForSeconds(animTime * loops);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator Setter()
        {
            Color actorColor = characterMesh.material.GetColor(shaderColorID);

            while (animating)
            {
                Color updated = new(actorColor.r, actorColor.g, actorColor.b, currentAlpha);

                characterMesh.material.SetColor(shaderColorID, updated);

                yield return new WaitForSeconds(Time.deltaTime * 2);
            }

            UnityEngine.Debug.Log($"{ToString()}: Setter Finished!");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveAnimation()
        {
            characterMesh.material = originalMaterial;

            material.mainTexture = null;

            material.SetColor(shaderColorID, defaultColor);

            animating = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void OnDisable() => animating = false;
    }
}