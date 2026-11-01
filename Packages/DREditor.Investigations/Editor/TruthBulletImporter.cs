//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Toolbox;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Investigations.Editor
{
    public class TruthBulletImporter : GenerateWizardBase
    {
        [Tooltip("")]
        [TextArea(5, 10)]
        [SerializeField] protected string textArea = "";

        [Tooltip("")]
        public string regexString = @"(.*?)(\r?\n//[\s\S]*?)(?=\r?\n\r?\n|\r?$)";

        [Tooltip("")]
        public EvidenceDatabase database;

        [Tooltip("")]
        public List<EvidenceKeywords> keys = new() { EvidenceKeywords.Name, EvidenceKeywords.Description, };


        public TruthBulletImporter() : base(true) => assetPrefix = "Bullet_";


        [MenuItem("DREditor/Import/Truth Bullets")]
        public static void CreateWizard() => DisplayWizard<TruthBulletImporter>("Import TruthBullets", "Import");

        protected override bool OnEnableWizardValid() => ImportValid();

        protected override bool ImportValid()
        {
            database = DatabaseUtility.LoadDatabase<EvidenceDatabase>();

            bool result = database != null || DatabaseUtility.TryCreateDatabaseAt("Resources/Evidence", out database);

            if (!result &&
                EditorUtility.DisplayDialog("Evidence Database was not specifid.",
                "Please create and assign a Evidence Database and try again", "OK"))
                Close();

            return result;
        }

        protected override void Import()
        {
            List<TruthBullet> evidenceToSave = new();

            if (string.IsNullOrEmpty(textArea) &&
                EditorUtility.DisplayDialog("No Text Sent",
                "Please paste the text to import in the text field.", "OK"))
                return;

            using StringReader reader = new(textArea);

            Regex regex = new(regexString);

            Line lastLine = null;

            int currentLine = 0;

            while (reader.ReadLine() is string line && line != null)
            {
                TruthBullet truthBullet = CreateInstance<TruthBullet>();

                if (!line.Contains("//"))
                    while (reader.ReadLine() is string l && l.Contains("//"))
                        line += "\n" + l;

                if (line.Trim().Length != 0)
                {
                    if (regex.IsMatch(line))
                    {
                        MatchBullet(regex.Match(line), truthBullet);

                        evidenceToSave.Add(truthBullet);
                    }
                    else if (lastLine == null &&
                            EditorUtility.DisplayDialog("Format incorrect", "The file didn't comply with the format\n" +
                                "on line " + currentLine + " \n Line: " + line, "OK"))
                        return;

                    currentLine++;
                }
            }

            CreateTruthBullet(evidenceToSave);
        }

        protected virtual void MatchBullet(Match m, TruthBullet truthBullet)
        {
            for (int i = 1; i < m.Groups.Count; i++)
            {
                var group = m.Groups[i];

                var keyword = keys[i - 1];

                switch (keyword)
                {
                    case EvidenceKeywords.Name:

                        truthBullet.DisplayName = group.Value;

                        break;

                    case EvidenceKeywords.Description:

                        truthBullet.Description = group.Value.Replace("//", "").Replace("\n", "");

                        break;
                }
            }
        }


        protected virtual void CreateTruthBullet(List<TruthBullet> evidenceToSave)
        {
            // Save the dialogue objects
            string folder = FolderUtility.CreateIntermediateFolders(targetDirectory);

            AssetDatabase.StartAssetEditing();

            for (int i = 0; i < evidenceToSave.Count; i++)
            {
                TruthBullet tb = evidenceToSave[i];

                string diaPath = folder + "/" + assetPrefix + tb.DisplayName.Replace(" ", "_") + ".asset";

                AssetDatabase.CreateAsset(tb, diaPath);
            }

            AssetDatabase.StopAssetEditing();

            AssetDatabase.SaveAssets();
        }
    }

    public enum EvidenceKeywords
    {
        Name, Description
    }
}
#endif