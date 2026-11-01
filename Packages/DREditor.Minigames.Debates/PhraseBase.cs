//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class PhraseBase
    {
        /// <summary>
        ///
        /// </summary>
        [TextArea]
        [SerializeField] protected string phraseText;

        /// <summary>
        ///
        /// </summary>
        public string Text { get => phraseText; set => phraseText = value; }

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected float phraseSize = 5;

        /// <summary>
        ///
        /// </summary>
        public float PhraseSize { get => phraseSize; set => phraseSize = value; }

        /// <summary>
        ///
        /// </summary>
        [SerializeReference]
        [FormerlySerializedAs("components")]
        public List<IPhraseModifier> modifiers = new();

        /// <summary>
        ///
        /// </summary>
        [SerializeReference]
        public List<IPhraseAnimationArchitecture> animations = new();


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool Has<T>() where T : IPhraseModifier
        {
            foreach (var component in modifiers)
                if (component is T)
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public bool Has<T>(out T modifier) where T : IPhraseModifier
        {
            modifier = default;

            foreach (var component in modifiers)
                if (component is T modifierComponent)
                {
                    modifier = modifierComponent;

                    return true;
                }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Has(Type type)
        {
            foreach (var component in modifiers)
                if (type.IsInstanceOfType(component))
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : IPhraseModifier
        {
            foreach (var component in modifiers)
                if (component is T componentInstance)
                    return componentInstance;

            return default;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGetModifier<T>([NotNullWhen(true)] out T result) where T : IPhraseModifier
        {
            result = modifiers.OfType<T>().FirstOrDefault();

            return result != null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newModifiers"></param>
        public virtual void ApplyModifiers(List<IPhraseModifier> newModifiers)
        {
            foreach (var newModifier in newModifiers)
                if (!modifiers.Any(modifier => modifier?.GetType() == newModifier?.GetType()))
                    modifiers.Add(newModifier);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newAnimations"></param>
        public virtual void ApplyAnimations(List<IPhraseAnimationArchitecture> newAnimations)
        {
            foreach (var animation in animations)
                foreach (var newAnimation in newAnimations)
                    if (animation?.GetType() == newAnimation?.GetType())
                        animation.Apply(newAnimation);

            foreach (var newAnimation in newAnimations)
                if (!animations.Any(animation => animation?.GetType() == newAnimation?.GetType()))
                    animations.Add(newAnimation);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        ///
        /// </summary>
        public virtual void Convert() { }
    }
}