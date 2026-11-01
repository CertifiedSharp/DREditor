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
    [CustomClassEditor(typeof(PhraseScaleType))]
    public class PhraseScaleTypeEditor : CompactPhraseTypeEditor<PhraseScaleType, Vector3>
    {
        public override string TypeName => "Scale";

        public override Func<string, Vector3, int, int, Vector3> FieldFunc => HandyFields.Vector3Field;
    }
}
#endif