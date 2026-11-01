//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Progressing;
using DREditor.Services;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RoomManagerBase : ServiceHolder<RoomManagerBase>
    {
#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        public int intChapter = 0;

        /// <summary>
        ///
        /// </summary>
        public Chapter Chapter = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="intChapter"></param>
        public virtual void SetIntChapter(int intChapter) => this.intChapter = intChapter;

        /// <summary>
        ///
        /// </summary>
        /// <param name="Chapter"></param>
        public virtual void SetChapter(Chapter Chapter) => this.Chapter = Chapter;

        /// <summary>
        ///
        /// </summary>
        /// <param name="onRoomInstanceLoaded"></param>
        public virtual void SetOnRoomInstanceLoaded(BoolWithEvent onRoomInstanceLoaded) => this.onRoomInstanceLoaded = onRoomInstanceLoaded;
#endif

        /// <summary>
        ///
        /// </summary>
        public BoolWithEvent onRoomInstanceLoaded = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterNum"></param>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public abstract bool HasBase(int chapterNum, object pValue);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public abstract object SaveRoomState(object pValue);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="instanceData"></param>
        public abstract void LoadRoomState(object pValue, object instanceData = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public abstract bool BaseIsPersistent(object pValue);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inEditor"></param>
        public abstract void UnloadAll(bool inEditor);
    }
}