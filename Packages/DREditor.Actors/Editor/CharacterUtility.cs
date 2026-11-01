//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Dialogues;
using DREditor.Editor.Utility;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Actors.Editor
{
    /// <summary>
    ///
    /// </summary>
    public static class CharacterUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [MenuItem("DREditor/Characters/Sort Expressions by their numbers")]
        public static void SortSelectedCharactersExpressionSprites()
        {
            UnityEngine.Debug.Log("REPORT: Starting sort.");

            var selections = Selection.objects;

            foreach (var obj in selections)
            {
                if (obj is Character character)
                {
                    character.SetExpressions(character.Expressions.OrderBy(expression => PadNumbers(expression.Name)).ToList());

                    character.SetSprites(character.Sprites.OrderBy(unlit => PadNumbers(unlit.Name)).ToList());

                    EditorUtility.SetDirty(character);
                }
            }

            UnityEngine.Debug.Log("REPORT: Sorting complete. If a Character asset was selected it has been set dirty.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string PadNumbers(string input) => Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(10, '0'));


        /// <summary>
        /// 
        /// </summary>
        [MenuItem("DREditor/Characters/Create Dialogue With All Sprites", true)]
        public static bool ValidateCreateDialogueWithAllSprites() =>
            Selection.objects.OfType<Character>().Count() > 0;

        /// <summary>
        /// 
        /// </summary>
        [MenuItem("DREditor/Characters/Create Dialogue With All Sprites")]
        public static void CreateDialogueWithAllSprites()
        {
            AssetDatabase.StartAssetEditing();

            DatabaseUtility.InitializeDatabase(null, out CharacterDatabase database);

            if (database == null)
                database = Resources.Load<CharacterDatabase>("Characters/CharacterDatabase");

            Character selectedCharacter = Selection.objects.OfType<Character>().First();

            Dialogue dialogue = CreateCharacterDialogueAsset<Dialogue>(
                folderPath: "Dialogues/Debugging/Sprites",
                assetName: "D_" + selectedCharacter.name + "_Debug_Sprites");

            int speakerIndex = database.Characters.IndexOf(selectedCharacter);

            dialogue.lines.Add(new Line(selectedCharacter, speakerIndex));

            new DialogueDebugToolEditor<Line>().ReplaceWithFirstLineCharactersSprites(dialogue, true);

            AssetDatabase.StopAssetEditing();

            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <param name="assetName"></param>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public static T CreateCharacterDialogueAsset<T>(string folderPath, string assetName, T dialogue = null) where T : DialogueCore
        {
            if (dialogue == null)
                dialogue = (T)ScriptableObject.CreateInstance(typeof(T));

            using (new AssetDatabase.AssetEditingScope())
            {
                dialogue.SetName(assetName);

                string assetPath = FolderUtility.CreateIntermediateFolders(folderPath) + "/" + assetName + ".asset";

                AssetDatabase.CreateAsset(dialogue, assetPath);
            }

            AssetDatabase.SaveAssets();

            return dialogue;
        }
    }
}
