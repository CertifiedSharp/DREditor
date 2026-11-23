//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System.Linq;

namespace DREditor
{
    /// <summary>
    /// Base class for a custom POCO editor.
    /// </summary>
    public abstract class ClassEditorBase
    {
        /// <summary>
        /// The target class object being edited as an object type.
        /// </summary>
        protected object baseTarget;

        /// <summary>
        /// Additionally sent parameters in order to assist the editing process of the base target.
        /// </summary>
        protected object[] parameters;


        /// <summary>
        /// Called from a Class Editor Factory through a string method name!
        /// Initializes the sent target and additional parameters and then displays the GUI.
        /// </summary>
        /// <param name="target">The target class object being edited as an object type.</param>
        /// <param name="objects">Additionally sent parameters in order to assist the editing process of the base target.</param>
        public virtual void OnGUI(object target, params object[] objects)
        {
            InitializeTarget(target, objects);

            OnInspectorGUI();
        }

        /// <summary>
        /// Initializes the sent target and additional parameters.
        /// </summary>
        /// <param name="target">The target class object being edited as an object type.</param>
        /// <param name="objects">Additionally sent parameters in order to assist the editing process of the base target.</param>
        private void InitializeTarget(object target, object[] objects)
        {
            baseTarget = target;

            parameters = objects;
        }

        /// <summary>
        /// Displays the GUI for the targeted POCO.
        /// </summary>
        protected abstract void OnInspectorGUI();


        /// <summary>
        /// Helper function to get the first found sent parameter of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to cast the first initialized parameter to use.</typeparam>
        /// <returns>The first available cached parameter of type <typeparamref name="T"/>.</returns>
        protected virtual T GetFirstParameter<T>() => parameters.OfType<T>().FirstOrDefault(item => item != null);
    }
}
#endif