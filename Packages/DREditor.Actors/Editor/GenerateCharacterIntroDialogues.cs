//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Editor.Utility;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Actors.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class GenerateCharacterIntroDialogues : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool generateAnimations = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool chainDirectTo = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useSeparateObjects = false;


        /// <summary>
        ///
        /// </summary>
        protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        protected Dialogue dialogue;

        /// <summary>
        ///
        /// </summary>
        protected Dialogue previousDialogue = null;

        /// <summary>
        ///
        /// </summary>
        protected Character character = null;

        /// <summary>
        /// 
        /// </summary>
        protected readonly string outputFolderPath = "Dialogues/Debugging/Intros";


        public GenerateCharacterIntroDialogues() => characterDatabase = DatabaseUtility.LoadDatabase<CharacterDatabase>();


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Characters/Generate Character Intro Debug Dialogue")]
        public static void GenerateIntroDialogue()
            => DisplayWizard<GenerateCharacterIntroDialogues>("Generate Character Intro Debug Dialogue", "Create");


        protected virtual void OnWizardCreate()
        {
            if (generateAnimations)
                GenerateAnimations();
            else
                GenerateDialogues();
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void GenerateAnimations()
        {
            foreach (var character in characterDatabase.Characters)
            {
                AnimationClip animationClip = new()
                {
                    name = character.FirstName
                };

                CreateAnimationClipAsset(animationClip, animationClip.name);
            }

            UnityEngine.Debug.LogWarning("REPORT: Character Animations have been Generated. Please right-click, create a new Animator Controller, " +
                "and drag the generated animations into the Animator window.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        protected virtual void CreateAnimationClipAsset(UnityEngine.Object obj, string name)
        {
            string folder = "Assets/Dialogues";

            try
            {
                AssetDatabase.StartAssetEditing();

                string diaPath = folder + "/" + name + ".asset";

                AssetDatabase.CreateAsset(obj, diaPath);

                AssetDatabase.StopAssetEditing();

                AssetDatabase.SaveAssets();
            }
            catch (Exception exception)
            {
                AssetDatabase.StopAssetEditing();

                UnityEngine.Debug.LogException(exception);
            }
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void GenerateDialogues()
        {
            dialogue = (Dialogue)ScriptableObject.CreateInstance(typeof(Dialogue));

            previousDialogue = null;

            foreach (Character character in characterDatabase.Characters)
                GenerateCharacterIntro(character);

            if (!useSeparateObjects)
                CreateIntroDialogueAsset(null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        protected virtual void GenerateCharacterIntro(Character character)
        {
            if (useSeparateObjects)
                dialogue = (Dialogue)ScriptableObject.CreateInstance(typeof(Dialogue));

            AddStartLine();

            AddIntroLine();

            AddEndLine();

            ChainDirectTo();

            if (useSeparateObjects)
                CreateIntroDialogueAsset(character);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddStartLine()
        {
            Line line = new(character, characterDatabase.Characters.IndexOf(character), "Test for intro.");

            dialogue.Lines.Add(line);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddIntroLine()
        {
            Line introLine = new();

            introLine.SetSpeakerNum(-1);

            introLine.DiaEvents.Add(new CharIntro(character.FirstName));

            introLine.ShowDiaEvents = true;

            dialogue.Lines.Add(introLine);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddEndLine()
        {
            Line endLine = new(character, characterDatabase.Characters.IndexOf(character), "End Test for intro.");

            dialogue.Lines.Add(endLine);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ChainDirectTo()
        {
            if (chainDirectTo)
            {
                if (previousDialogue != null)
                {
                    previousDialogue.DirectTo.SetEnabled(true);

                    previousDialogue.DirectTo.SetNewDialogue(dialogue);
                }

                previousDialogue = dialogue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        protected virtual void CreateIntroDialogueAsset(Character character)
        {
            string assetName = "D_" + (character != null ? character.FirstName : "All") + "_Debug_Intros";

            CharacterUtility.CreateCharacterDialogueAsset(outputFolderPath, assetName, dialogue);
        }
    }
}