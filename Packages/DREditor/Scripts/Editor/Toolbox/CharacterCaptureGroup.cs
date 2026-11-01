//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Editor.Toolbox
{
    [Serializable]
    public class CharacterCaptureGroup : IDialogueCaptureGroup, IImportCompleteResponse, IValidCaptureGroup
    {
        private enum NameMatchOption
        {
            FullName, FirstNameOnly, LastNameOnly
        }

        [Tooltip("")]
        [SerializeField] private NameMatchOption nameMatch = NameMatchOption.FirstNameOnly;

        [ShowIf(nameof(GroupValid))]
        [Tooltip("")]
        [FormerlySerializedAs("database")]
        [SerializeField] protected CharacterDatabase characterDatabase;


        public bool IsPriority => true;

        protected bool showWarnCharactersNotFound = false;

        public virtual bool GroupValid()
        {
            bool result = DatabaseUtility.InitializeDatabase(characterDatabase, out characterDatabase);

            if (!result)
                EditorUtility.DisplayDialog("CharacterDatabase was not specified.",
                "Please create and assign a CharacterDatabase to use: " + nameof(CharacterCaptureGroup), "OK");

            return result;
        }

        public virtual void Capture<T>(ref T line, string capturedText) where T : Line
        {
            line.SetSpeaker(FindCharacterInDB(capturedText, out int speakerIndex, out int aliasIndex));

            line.SetSpeakerNum(speakerIndex);

            line.SetAliasNumber(aliasIndex);
        }

        public void OnImportComplete()
        {
            if (showWarnCharactersNotFound)
                EditorUtility.DisplayDialog("Some character were not found", "Some characters couldn't be found on the database.", "OK");
        }

        protected Character FindCharacterInDB(string name, out int index, out int alias)
        {
            index = -1;

            alias = -1;

            Character found = characterDatabase.Characters[0];

            for (int i = 0; i < characterDatabase.Characters.Count; i++)
            {
                Character stu = characterDatabase.Characters[i];

                if (MatchedName(name, stu) || MatchedAlias(name, stu, out alias))
                {
                    found = stu;

                    index = i;

                    break;
                }
            }

            if (index == -1)
            {
                index = 0;

                showWarnCharactersNotFound = true;
            }

            return found;
        }

        protected virtual bool MatchedAlias(string name, Character stu, out int alias)
        {
            alias = -1;

            bool result = name.Contains(stu.FirstName) && stu.Aliases.Count > 0;

            if (result)
                for (int j = 0; j < stu.Aliases.Count; j++)
                    if (name.Contains(stu.Aliases[j].Name))
                        alias = j;

            return result;
        }

        protected virtual bool MatchedName(string name, Character character)
        {
            string soughtName = name.Trim().ToLowerInvariant();

            string lastFirst = character.LastName + " " + character.FirstName;

            string firstLast = character.FirstName + " " + character.LastName;

            switch (nameMatch)
            {
                case NameMatchOption.FirstNameOnly:
                    return character.FirstName.Trim().ToLowerInvariant().Equals(soughtName);
                case NameMatchOption.LastNameOnly:
                    return character.LastName.Trim().ToLowerInvariant().Equals(soughtName);
                case NameMatchOption.FullName:
                    return lastFirst.Trim().ToLowerInvariant().Equals(soughtName)
                        || firstLast.Trim().ToLowerInvariant().Equals(soughtName);
            }

            return false;
        }
    }
}