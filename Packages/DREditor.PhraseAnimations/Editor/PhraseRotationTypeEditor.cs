//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System;
using UnityEngine;

namespace DREditor.PhraseAnimations.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomClassEditor(typeof(PhraseRotationType))]
    public class PhraseRotationTypeEditor : CompactPhraseTypeEditor<PhraseRotationType, Vector4>
    {
        public override string TypeName => "Rotation";

        public override Func<string, Vector4, int, int, Vector4> FieldFunc => HandyFields.Vector4Field;
    }
}
#endif