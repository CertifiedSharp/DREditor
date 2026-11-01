//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System;

namespace DREditor.PhraseAnimations.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomClassEditor(typeof(PhraseFadeType))]
    public class PhraseFadeTypeEditor : CompactPhraseTypeEditor<PhraseFadeType, float>
    {
        public override string TypeName => "Fade";

        public override Func<string, float, int, int, float> FieldFunc => HandyFields.FloatField;
    }
}
#endif