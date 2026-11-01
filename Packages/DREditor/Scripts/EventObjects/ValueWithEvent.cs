// Author: Bart Heijltjes (Wispfire)
// EventObjects - A scriptable-object based messaging system for Unity
//
// Copyright (c) 2019 Bart Heijltjes (Wispfire)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.EventObjects
{
    public abstract class BaseValueWithEvent : ScriptableObject
    {
        public bool debug = false;

        public abstract void Invoke();
        public abstract bool IsInitialized { get; protected set; }
        public abstract void Reset();
        public abstract void Init();

        public UnityEvent OnUpdated = new();


        public virtual void AddListener(UnityAction action)
        {
            if (debug)
                UnityEngine.Debug.Log(name + ": OnUpdated has been subscribed to.");

            OnUpdated.AddListener(action);
        }

        public virtual void RemoveListener(UnityAction action)
        {
            if (debug)
                UnityEngine.Debug.Log(name + ": OnUpdated has been unsubscribed to.");

            OnUpdated.RemoveListener(action);
        }

        protected virtual void InvokeUpdated() => OnUpdated?.Invoke();
    }
    
    public abstract class ValueWithEvent<T,TY> : BaseValueWithEvent where TY : UnityEvent<T>, new()
    {
        public bool alwaysInvokeOnSetValue = false;

        public TY OnChange = new();

        public T InitialValue;

        public override bool IsInitialized { get; protected set; }

        [SerializeField] private T _value;
        public T Value
        {
            get
            {
                if (!IsInitialized) Init();
                return _value;
            }
            set
            {
                SetValue(value);
                
                if (debug)
                    UnityEngine.Debug.Log(name + " " + value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void SetValue(T x)
        {
            if (!IsInitialized) IsInitialized = true;

            if (x != null)
            {
                if (alwaysInvokeOnSetValue || !x.Equals(_value))
                {
                    _value = x;

                    OnChange.Invoke(x);

                    InvokeUpdated();
                }
            }
            else
            {
                _value = default(T); 
            }
            
            if (debug)
                UnityEngine.Debug.Log(name + "'s value set to: " + _value);
        }

        /// <summary>
        ///
        /// </summary>
        public override void Invoke()
        {
            OnChange.Invoke(Value);

            InvokeUpdated();
        }

        /// <summary>
        ///
        /// </summary>
        public override void Init()
        {
            _value = InitialValue;
            IsInitialized = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public virtual T GetValueAndAddListener(UnityAction<T> action)
        {
            if (!IsInitialized) 
                Init();

            AddListener(action);

            return Value;
        }

        public virtual void AddListener(UnityAction<T> action)
        {
            if (debug)
                UnityEngine.Debug.Log(name + ": OnChange has been subscribed to.");

            OnChange.AddListener(action);
        }

        public virtual void RemoveListener(UnityAction<T> action)
        {
            if (debug)
                UnityEngine.Debug.Log(name + ": OnChange has been unsubscribed to.");

            OnChange.RemoveListener(action);
        }

        /// <summary>
        ///
        /// </summary>
        public override void Reset()
        {
            _value = default(T);
            IsInitialized = false;
        }

        // In the editor, reset IsInitialized value when changing playmode.
#if UNITY_EDITOR
        void OnEnable()
        {
            UnityEditor.EditorApplication.playModeStateChanged += PlayModeStateChange;
        }

        void OnDisable()
        {
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeStateChange;

        }

        private void PlayModeStateChange(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode || state == UnityEditor.PlayModeStateChange.ExitingEditMode) Reset();
        }
#endif
        
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TX"></typeparam>
    /// <typeparam name="TY"></typeparam>
    // TODO: Add option to use local value instead of BoolWithEvent
    public class CachedVariable<T,TX,TY> where T : ValueWithEvent<TX, TY> where TY : UnityEvent<TX>, new() 
    {        
        public T SyncWith;

        private TX _currentValue;
        private bool _isSetup;

        public TX Value
        {
            get => _currentValue;
            set => SyncWith.Value = value;
        }

        /// <summary>
        ///
        /// </summary>
        public void Register()
        {
            if (_isSetup) return;
            if (SyncWith == null)
            {
                UnityEngine.Debug.LogWarning("SyncWith field is not filled. This is not supported.");
            }
            
            _currentValue = SyncWith.GetValueAndAddListener((x) => _currentValue = x);
            _isSetup = true;
        }
        
        /// <summary>
        ///
        /// </summary>
        public void Unregister()
        {
            SyncWith.RemoveListener(Sync);
            _isSetup = false;
        }

        void Sync(TX value)
        {
            _currentValue = value;
        }
        
    }
    
    
    [Serializable] public class BoolEvent : UnityEvent<bool>{}
    [Serializable] public class IntEvent : UnityEvent<int>{}
    [Serializable] public class FloatEvent : UnityEvent<float>{}
    [Serializable] public class SpriteEvent : UnityEvent<Sprite>{}
    [Serializable] public class TextureEvent : UnityEvent<Texture2D>{}
    [Serializable] public class StringEvent : UnityEvent<string>{}
    [Serializable] public class StringArrayEvent : UnityEvent<string[]>{}
    [Serializable] public class GameObjectEvent : UnityEvent<GameObject>{}
    
    [Serializable] public class TransformEvent : UnityEvent<PosRot>{}
}
