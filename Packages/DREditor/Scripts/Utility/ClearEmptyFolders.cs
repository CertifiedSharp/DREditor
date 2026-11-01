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
    ///
    /// </summary>
    public class RemoveEmptyFolders : AssetModificationProcessor
    {
        /// <summary>
        ///
        /// </summary>
        static readonly StringBuilder s_Log = new();

        /// <summary>
        ///
        /// </summary>
        static readonly List<DirectoryInfo> s_Results = new();


        /// <summary>
        ///
        /// </summary>
        [MenuItem("Assets/Remove Empty Folders")]
        public static void Remove()
        {
            s_Results.Clear();

            var assetsDir = Application.dataPath + Path.DirectorySeparatorChar;

            GetEmptyDirectories(new DirectoryInfo(assetsDir), s_Results);

            if (0 < s_Results.Count)
            {
                s_Log.Length = 0;

                s_Log.AppendFormat("Remove {0} empty directories as following:\n", s_Results.Count);

                foreach (var d in s_Results)
                {
                    s_Log.AppendFormat("- {0}\n", d.FullName.Replace(assetsDir, ""));

                    FileUtil.DeleteFileOrDirectory(d.FullName);
                    FileUtil.DeleteFileOrDirectory(d.FullName + ".meta");
                }

                s_Log.Length = Mathf.Min(s_Log.Length, 15000);

                Debug.Log(s_Log.ToString());

                s_Log.Length = 0;

                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        private static bool GetEmptyDirectories(DirectoryInfo dir, List<DirectoryInfo> results)
        {
            bool isEmpty = true;

            try
            {
                isEmpty = dir.GetDirectories().Count(x => !GetEmptyDirectories(x, results)) == 0 
                        && dir.GetFiles("*.*").All(x => x.Extension == ".meta");
            }
            catch
            {

            }

            if (isEmpty)
                results.Add(dir);

            return isEmpty;
        }
    }
}
#endif