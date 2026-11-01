//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;
using System.Linq;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CGStateLoaderBase : DialogueComponent, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        protected bool inLoadCGState = false;

        /// <summary>
        ///
        /// </summary>
        protected CGStateLoaderBase[] cachedLoaders;


        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearAllOtherCGLoaders()
        {
            cachedLoaders ??= FindObjectsByType<CGStateLoaderBase>().Where(loader => loader != this).ToArray();

            foreach (var loader in cachedLoaders)
                loader.ClearTracking();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearTracking() => SetLoadingCGState(false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadingState"></param>
        protected virtual void SetLoadingCGState(bool loadingState) => inLoadCGState = loadingState;


        /// <summary>
        ///
        /// </summary>
        public virtual void ToTitleScreen() => ClearTracking();
    }
}