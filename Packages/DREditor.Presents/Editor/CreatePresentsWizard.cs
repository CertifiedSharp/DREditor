//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class CreatePresentsWizard : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int startID = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool spriteHasIDNum = false;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(IDWillBeInSpriteName))]
        [Tooltip("")]
        [SerializeField] protected string matchSpriteID = @"\d+";


        /// <summary>
        ///
        /// </summary>
        protected CharacterDatabase database;


        [MenuItem("DREditor/FTE/Create Presents")]
        static void CreateWizard() => DisplayWizard<CreatePresentsWizard>("Create Materials", "Create");

        protected virtual void OnEnable() => database = DatabaseUtility.LoadDatabase<CharacterDatabase>();

        protected virtual void OnWizardCreate()
        {
            try
            {
                AssetDatabase.StartAssetEditing();

                CreatePresents();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();

                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreatePresents()
        {
            var textures = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets).Cast<Texture2D>();

            int id = startID;

            foreach (var texture in textures)
            {
                string path = AssetDatabase.GetAssetPath(texture);

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                path = path[..path.LastIndexOf(".")] + ".asset";

                if (PresentAlreadyExists(path))
                    continue;

                CreatePresent(path, id, sprite);

                id += 1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual bool PresentAlreadyExists(string path)
        {
            bool result = AssetDatabase.LoadAssetAtPath(path, typeof(Present)) != null;

            if (result)
                UnityEngine.Debug.LogWarning("ALERT: Can't create present, it already exists: " + path);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <param name="image"></param>
        protected virtual void CreatePresent(string path, int id, Sprite image)
        {
            List<Character> characters = database.GetFTECharacterList();

            var present = CreateInstance<Present>();

            present.DisplayName = image.name;

            present.ID = EvaluatePresentID(image, id);

            present.Image = image;

            StudentReactionCollection collection = new();

            foreach (Character character in characters)
            {
                StudentReaction reaction = new()
                {
                    Character = character
                };

                collection.studentReactions.Add(reaction);
            }

            present.attributes.Add(collection);

            AssetDatabase.CreateAsset(present, path);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual int EvaluatePresentID(Sprite sprite, int id)
            => IDWillBeInSpriteName() ? Int32.Parse(Regex.Match(sprite.name, matchSpriteID).Value) : id;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool IDWillBeInSpriteName() => spriteHasIDNum;
    }
}
#endif