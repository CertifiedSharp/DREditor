//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;

namespace DREditor.RoomBuilding.Old
{
    /// <summary>
    ///
    /// </summary>
    public class RoomManagerShellDisplayEditor : RoomShellDisplayEditorBase
    {
        readonly RoomManager manager;

        readonly RoomBuilder builder;


        public RoomManagerShellDisplayEditor(RoomManager manager, RoomBuilder builder)
        {
            this.manager = manager;

            this.builder = builder;
        }
        /// <inheritdoc/>
        protected override bool HasShellData() => builder.Shells.Count != 0;

        /// <inheritdoc/>
        protected override void CreateShellSave() => builder.Shells.Add(manager.NewSaveRoom(null));

        /// <inheritdoc/>
        protected override void LoadShell()
        {
            ActorLoader.UnloadActors(true);

            manager.LoadRoom(builder.Shells[0]);
        }

        /// <inheritdoc/>
        protected override void OverwriteShell() => builder.Shells[0] = manager.OverwriteRoom(builder.Shells[0]);

        /// <inheritdoc/>
        protected override void DeleteShell()
        {
            builder.Shells.Clear();

            EditorUtility.SetDirty(builder);
        }
    }
}
#endif