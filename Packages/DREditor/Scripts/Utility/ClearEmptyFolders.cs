//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    /// Class to Remove Empty Project folders to keep projects clean.
    /// </summary>
    public class RemoveEmptyFolders : AssetModificationProcessor
    {
        /// <summary>
        /// The Builder to create the log since the entire project is being scanned.
        /// </summary>
        private static readonly StringBuilder Log = new();

        /// <summary>
        /// The list of found empty directories after scanning that will be deleted.
        /// </summary>
        private static readonly List<DirectoryInfo> Results = new();

        /// <summary>
        /// The string path to the projects Assets Directory.
        /// </summary>
        private static readonly string AssetsDirectory = Application.dataPath + Path.DirectorySeparatorChar;


        /// <summary>
        /// Removes Empty Project folders to keep projects clean.
        /// </summary>
        [MenuItem("DREditor/Utility/Remove Empty Project Folders")]
        public static void RemoveEmptyProjectFolders()
        {
            InitializeEmptyDirectoriesFrom(new DirectoryInfo(AssetsDirectory));

            if (ProjectHasEmptyDirectories())
                RemoveEmptyDirectories();
        }

        /// <summary>
        /// Recursively scans <paramref name="directory"/> for empty folders, adding any to <see cref="Results"/>.
        /// </summary>
        /// <param name="directory">The directory to start from.</param>
        /// <returns>True if <paramref name="directory"/> and all subdirectories are empty, otherwise false.</returns>
        private static bool InitializeEmptyDirectoriesFrom(DirectoryInfo directory)
        {
            Results.Clear();

            bool isEmpty = true;

            isEmpty = directory.GetDirectories().Count(directoryInfo => !InitializeEmptyDirectoriesFrom(directoryInfo)) == 0 &&
                directory.GetFiles("*.*").All(x => x.Extension == ".meta");

            if (isEmpty)
                Results.Add(directory);

            return isEmpty;
        }


        /// <summary>
        /// Checks <see cref="Results"/> has any empty directories.
        /// </summary>
        /// <returns>True if <see cref="Results"/>'s count is greater than Zero.</returns>
        private static bool ProjectHasEmptyDirectories() => Results.Count > 0;

        /// <summary>
        /// Removes All Empty Directories from Initialization on <see cref="Results"/>.
        /// </summary>
        private static void RemoveEmptyDirectories()
        {
            InitializeLog();

            DeleteEmptyFolders();

            ResolveLog();

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Starts up the string builder for to log the removed folders to the console.
        /// </summary>
        private static void InitializeLog()
        {
            Log.Length = 0;

            Log.AppendFormat("Remove {0} empty directories as following:\n", Results.Count);
        }

        /// <summary>
        /// Records all empty directories to the log before deleting them.
        /// </summary>
        private static void DeleteEmptyFolders()
        {
            foreach (DirectoryInfo emptyDirectory in Results)
                DeleteEmptyFolder(emptyDirectory);
        }

        /// <summary>
        /// Records the empty directory to the log before deleting it.
        /// </summary>
        /// <param name="emptyDirectory">The directory to delete.</param>
        private static void DeleteEmptyFolder(DirectoryInfo emptyDirectory)
        {
            Log.AppendFormat("- {0}\n", emptyDirectory.FullName.Replace(AssetsDirectory, ""));

            FileUtil.DeleteFileOrDirectory(emptyDirectory.FullName);

            FileUtil.DeleteFileOrDirectory(emptyDirectory.FullName + ".meta");
        }

        /// <summary>
        /// Displays the log of what was deleted to the console and applies a minimum so that the log doesn't overflow the console.
        /// </summary>
        private static void ResolveLog()
        {
            Log.Length = Mathf.Min(Log.Length, 15000);

            Debug.Log(Log.ToString());

            Log.Length = 0;
        }
    }
}
#endif