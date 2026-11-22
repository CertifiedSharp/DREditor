//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
namespace DREditor
{
    /// <summary>
    /// Base class for a custom class editor for POCO of type: <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The target class type being edited.</typeparam>
    public abstract class ClassEditorTypeBase<T> : ClassEditorBase
    {
        /// <summary>
        /// The target <typeparamref name="T"/> being edited.
        /// </summary>
        protected T target;


        /// <inheritdoc/>
        public override void OnGUI(object target, params object[] objects)
        {
            this.target = (T)target;

            base.OnGUI(target, objects);
        }
    }
}
#endif