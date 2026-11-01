//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    public class RoomStateShellDisplayEditor : RoomShellDisplayEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        readonly RoomStateManager manager;

        /// <summary>
        ///
        /// </summary>
        readonly RoomStateBuilder builder;

        /// <summary>
        ///
        /// </summary>
        readonly RoomStateSaveDisplayEditor editor;


        public RoomStateShellDisplayEditor(RoomStateManager manager, RoomStateBuilder builder, RoomStateSaveDisplayEditor editor)
        {
            this.manager = manager;

            this.builder = builder;

            this.editor = editor;
        }


        /// <inheritdoc/>
        protected override void CreateShellSave()
        {
            string shellPath = FolderUtility.GetAssetFolderPath(builder);

            if (!FolderUtility.TryCreateFolder(ref shellPath, builder.name + "_Shell"))
                return;

            string builderPrefix = builder.name.Replace(".asset", "");

            string shellAssetName = builderPrefix + $"_{manager.intChapter}_Shell";

            if (!FolderUtility.TryCreateFolder(ref shellPath, shellAssetName))
                return;

            string newAssetPath = $"{shellPath}/{shellAssetName}.asset";

            RoomStateData shellAsset = editor.CreateRoomState(null, newAssetPath);

            CheckFillShells();

            if (manager.intChapter >= 0 && manager.intChapter < builder.Shells.Count)
                builder.Shells[manager.intChapter] = shellAsset;
            else
                builder.Shells.Add(shellAsset);
        }

        /// <inheritdoc/>
        protected override bool HasShellData()
        {
            if (builder == null)
                return false;

            bool chapterIsLessThanShellCount = manager.intChapter < builder.Shells.Count;

            if (!chapterIsLessThanShellCount)
                return false;

            bool shellListContainsValidShellAsset = builder.Shells[manager.intChapter] != null;

            return chapterIsLessThanShellCount && shellListContainsValidShellAsset;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckFillShells()
        {
            if (builder.Shells.Count < manager.intChapter)
            {
                builder.Shells.Add(null);

                CheckFillShells();
            }
        }

        /// <inheritdoc/>
        protected override void LoadShell() => editor.LoadRoom(builder.Shells[manager.intChapter]);

        /// <inheritdoc/>
        protected override void OverwriteShell() => editor.OverwriteRoom(null, builder.Shells[manager.intChapter], -1);

        /// <inheritdoc/>
        protected override void DeleteShell()
        {
            string shellPath = FolderUtility.GetAssetFolderPath(builder.Shells[manager.intChapter]);

            editor.DeleteRoomState(builder, shellPath, builder.Shells, manager.intChapter);
        }
    }
}
#endif