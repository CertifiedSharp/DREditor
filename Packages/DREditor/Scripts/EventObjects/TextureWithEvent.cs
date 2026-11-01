//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using System;
using UnityEngine;

namespace DREditor.EventObjects
{
    [CreateAssetMenu(menuName = "DREditor/Event Objects/Texture", fileName = "New TextureWithEvent")]
    public class TextureWithEvent : ValueWithEvent<Texture2D, TextureEvent>
    {
        public bool Resolve()
        {
            return Value; 
        }
    }

    [Serializable]
    public class TextureVariable : CachedVariable<TextureWithEvent, Texture2D, TextureEvent> { }
}