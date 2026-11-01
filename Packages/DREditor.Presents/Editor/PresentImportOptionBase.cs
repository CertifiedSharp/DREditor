//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class PresentImportOptionBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PresentDatabase presentDatabase;

        /// <summary>
        ///
        /// </summary>
        [TextArea(10, 50)]
        [Tooltip("")]
        [SerializeField] protected string textArea;

        /// <summary>
        ///
        /// </summary>
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected string anyNumberRegex = "\\D+";


        /// <summary>
        ///
        /// </summary>
        protected readonly string tab = "\t";

        /// <summary>
        ///
        /// </summary>
        protected Action Close;



        /// <summary>
        ///
        /// </summary>
        public abstract void Invoke();


        /// <summary>
        ///
        /// </summary>
        /// <param name="Close"></param>
        public virtual void Initialize(Action Close)
        {
            this.Close = Close;

            InitializeDatabases();

            CloseIfInitializationFailed();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeDatabases() => presentDatabase = DatabaseUtility.LoadDatabase<PresentDatabase>();

        /// <summary>
        ///
        /// </summary>
        protected virtual void CloseIfInitializationFailed()
        {
            if (InitializationFailed())
                Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitializationFailed()
        {
            bool result = !presentDatabase;

            if (result)
                EditorUtility.DisplayDialog("Missing Databases", "Please make sure you have a Present Database to " +
                    "use this functionality!", "OK");

            return result;
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual List<int> GetListOfIDs() => CollectIDs(Regex.Split(textArea, anyNumberRegex));

        /// <summary>
        ///
        /// </summary>
        /// <param name="matches"></param>
        /// <returns></returns>
        protected virtual List<int> CollectIDs(string[] matches)
        {
            List<int> ids = new();

            foreach (string matchedString in matches)
            {
                if (matchedString == string.Empty)
                    continue;

                int matchedInt = Int32.Parse(matchedString.Replace(tab, ""));

                if (ids.Contains(matchedInt))
                    continue;
                else
                    ids.Add(matchedInt);
            }

            return ids;
        }
    }
}
#endif