//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Backlogging
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class BacklogHandler : BacklogHandlerBase, IDRESaveable, IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Backlog backlog = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dataSaveKey = "BackData";


        /// <inheritdoc/>
        public override object[] GetBacklogLines() => backlog.GetLinesArray();

        /// <inheritdoc/>
        public override void ClearLines() => backlog.Lines.Clear();

        /// <inheritdoc/>
        public override void AddLine(object o = null)
        {
            if (o != null && o is BacklogLines line)
                backlog.AddLine(line.Charfile, line.Text, line.AliasNum, line.Color, line.Voice);
        }

        /// <inheritdoc/>
        public override object SaveBacklog() => backlog.Save();

        /// <inheritdoc/>
        public override void LoadBacklog(object o = null) => backlog.Load((BacklogData)o);

        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData) => saveData.Set(dataSaveKey, (BacklogData)SaveBacklog());

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => LoadBacklog(saveDatas.Get<BacklogData>(dataSaveKey));
    }
}