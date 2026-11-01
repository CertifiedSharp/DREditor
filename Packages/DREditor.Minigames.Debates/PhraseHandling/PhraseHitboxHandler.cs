//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Shattering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PhraseTextComponentBase) + ", " + nameof(IPhraseHandlerComponent) + ", " +
        "\n\n")]
    public class PhraseHitboxHandler : PhraseTextComponentBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoxCollider allTextBox;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoxCollider shatterCollider;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 onlyHitShatterBoxOffset = new(0, 0, 0.1f);


        /// <summary>
        ///
        /// </summary>
        protected IHitBoxOverride hitboxOverride;

        /// <summary>
        ///
        /// </summary>
        protected readonly List<BoxCollider> addedBoxes = new();


        /// <summary>
        ///
        /// </summary>
        protected override void InitializeComponents()
        {
            base.InitializeComponents();

            TryGetComponent(out hitboxOverride);
        }


        /// <inheritdoc/>
        public override void StartPhrase<T>(T phrase)
        {
            InitializeText(phrase);

            ClearHitBoxes();

            StartCoroutine(SetHitBox(phrase));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        protected virtual void InitializeText(PhraseBase phrase)
        {
            shatterCollider.enabled = phrase.Has<HittableModifierBase>();

            tmpText.autoSizeTextContainer = true;

            tmpText.ForceMeshUpdate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearHitBoxes()
        {
            allTextBox.center = Vector3.zero;

            shatterCollider.center = Vector3.zero;

            foreach (BoxCollider box in addedBoxes)
                Destroy(box);

            addedBoxes.Clear();

            tmpText.ForceMeshUpdate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        protected virtual IEnumerator SetHitBox(PhraseBase phrase)
        {
            yield return new WaitForEndOfFrame();

            GetComponentInChildren<IShatterText>().SetMesh();

            Vector3 shatterBoxOverride = hitboxOverride.SetHitBox(phrase, addedBoxes);

            if (shatterBoxOverride != Vector3.zero)
                allTextBox.size = shatterBoxOverride;
            else
                SetOnlyHitBox();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetOnlyHitBox()
        {
            allTextBox.size = tmpText.bounds.size;

            shatterCollider.size = allTextBox.size + onlyHitShatterBoxOffset;
        }
    }
}