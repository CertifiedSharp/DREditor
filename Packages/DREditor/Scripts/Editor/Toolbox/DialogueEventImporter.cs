//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Editor.Utility;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    using Debug = UnityEngine.Debug;
    //This shits getting reworked in the future.
    public class DialogueEventImporter
    {

        #region Adding Events
        bool cg = false;
        bool miniCG = false;
        //bool loadScene = false;
        bool endCG = false;
        bool endMiniCG = false;
        string cgName = "";
        public void EvalEvents(string l)
        {

            if (l.Contains("CG - ") && !l.Contains("Mini-CG - "))
            {
                cg = true;
                cgName = l.Replace("//CG - ", "");
            }
            if (l.Contains("Mini-CG - "))
            {
                miniCG = true;
                cgName = l.Replace("//Mini-CG - ", "");
            }
            if (l.Contains("End CG"))
            {
                endCG = true;
            }
            if (l.Contains("End Mini-CG"))
            {
                endMiniCG = true;
            }

        }
        public T AddEvents<T>(T l) where T : Line
        {
            if (endCG)
            {
                endCG = false;
                l.DiaEvents.Add(new CGDisplay());
            }
            if (endMiniCG)
            {
                endMiniCG = false;
                l.DiaEvents.Add(new ShowItem());
            }
            if (cg)
            {
                cg = false;
                Sprite s = null;
                try
                {
                    s = BuilderUtility.FindAssetsByType<Sprite>(cgName)[0];
                }
                catch
                {
                    Debug.LogWarning("An Error Occurred trying to import cg: " + cgName);
                    s = BuilderUtility.FindAssetsByType<Sprite>("Black Screen")[0];
                }
                if (s != null)
                {
                    l.DiaEvents.Add(new CGDisplay(s));
                    Debug.Log("Added " + cgName);

                }
                else
                {
                    Debug.LogWarning("FAILED TO IMPORT CG");
                }
            }
            if (miniCG)
            {
                miniCG = false;
                Texture2D s = null;
                try
                {
                    s = BuilderUtility.FindAssetsByType<Texture2D>(cgName)[0];
                }
                catch
                {
                    Debug.LogWarning("An Error Occurred trying to import Mini cg" + cgName);
                    s = BuilderUtility.FindAssetsByType<Texture2D>("Black Screen")[0];
                }

                if (s != null)
                {
                    l.DiaEvents.Add(new ShowItem(s));
                    Debug.Log("Added " + cgName);
                }
                else
                {
                    Debug.LogWarning("FAILED TO IMPORT MINICG");
                }
            }

            return l;
        }
        #endregion
    }
}
#endif