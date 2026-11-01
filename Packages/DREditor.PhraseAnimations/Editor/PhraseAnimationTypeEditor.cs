//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
namespace DREditor.PhraseAnimations.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PhraseAnimationTypeEditor<T> : PhraseTypeEditor where T : class
    {
        /// <summary>
        ///
        /// </summary>
        protected T typeTarget;


        protected override void OnInspectorGUI()
        {
            typeTarget = baseTarget as T;

            base.OnInspectorGUI();
        }
    }
}
#endif