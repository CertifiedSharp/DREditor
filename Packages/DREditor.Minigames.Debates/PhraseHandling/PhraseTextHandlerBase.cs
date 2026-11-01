//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using System.Linq;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PhraseTextHandlerBase : PhraseTextBase
    {
        /// <summary>
        ///
        /// </summary>
        [HideInInspector] public TMP_Text tmpText;


        /// <summary>
        ///
        /// </summary>
        protected IPhraseHandlerComponent[] phraseComponents;


        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            base.InitializeComponents();

            tmpText = InitializeComponent<TMP_Text>();

            phraseComponents = GetComponentsInChildren<IPhraseHandlerComponent>()
                .OrderBy(component => component.Priority)
                .ToArray();
        }

        /// <inheritdoc/>
        public override float Initialize<T>(T phrase)
        {
            base.Initialize(phrase);

            tmpText.text = phrase.Text;

            tmpText.fontSize = phrase.PhraseSize;

            // orient text near space of the camera
            transform.parent.SetPositionAndRotation(new Vector3(0, cameraTransform.position.y, 0),
                new Quaternion(0, cameraTransform.rotation.y, 0, cameraTransform.rotation.w));

            foreach (var component in phraseComponents)
                if (textPhrase is T typedPhrase)
                    component.StartPhrase(typedPhrase);
                else
                    UnityEngine.Debug.LogError($"ERROR: Expected {typeof(T)}, got {textPhrase.GetType()}");

            float duration = 0;

            foreach (var component in phraseComponents)
                if (component is IPhraseAnimationComponent animationComponent)
                    duration += animationComponent.GetPhraseDuration();

            return duration;
        }

        /// <inheritdoc/>
        public override void Reset()
        {
            tmpText.text = "";

            tmpText.autoSizeTextContainer = false;

            tmpText.transform.position = new(0, 0, 0);
#if DOTWEEN
            tmpText.DOKill();
#endif
            Color color = tmpText.color;

            color.a = 1;

            tmpText.color = color;
        }
    }
}