//Original Author: SeleniumSoul
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Characters
{
    
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Characters/Database", fileName = "CharacterDatabase", order = 0)]
    public class CharacterDatabase : ScriptableObject
    {
        [SerializeField]
        public List<Character> Characters = new List<Character>();


        public List<string> GetNames()
        {
            var names = new List<string>();

            foreach (var cha in Characters)
            {
                switch (cha)
                {
                    case Student _:
                        names.Add(cha.LastName + " " + cha.FirstName);
                        break;
                }
            }
            return names;
        }
        public List<int> GetInts()
        {
            var ints = new List<int>();

            for (int i = 0; i < Characters.Count; i++)
            {
                ints.Add(i);
            }
            return ints;
        }
        
        public Character GetCharacter(string firstName)
        {
            if (Characters.Count != 0)
            {
                foreach( Character c in Characters)
                {
                    if(c.FirstName == firstName)
                    {
                        return c;
                    }
                }
            }

            return null;
        }
        public Character GetCharacterByContains(string firstName)
        {
            if (Characters.Count != 0)
            {
                foreach (Character c in Characters)
                {
                    if (firstName.Contains(c.FirstName))
                    {
                        return c;
                    }
                }
            }

            return null;
        }
        public Character GetCharacterByContaining(string name)
        {
            if (Characters.Count != 0)
            {
                foreach (Character c in Characters)
                {
                    if (name.Contains(c.FirstName))
                    {
                        return c;
                    }
                }
            }

            return null;
        }
        public List<Character> GetFTECharacterList()
        {
            List<Character> list = new List<Character>();
            foreach (Character c in Characters)
            {
                if (!c.NotFTECharacter)
                {
                    list.Add(c);
                }
            }
            return list;
        }
        public int GetFTECharacterCount()
        {
            return GetFTECharacterList().Count;
        }

        public int GetActorPrefabCount()
        {
            int count = 0;
            foreach (Character c in Characters)
            {
                if (c.ActorPrefab != null)
                {
                    count++;
                }
            }
            return count;
        }

        public virtual bool IsValid()
        {
            if (Characters == null)
            {
                UnityEngine.Debug.LogWarning("Add at least one character in the CharacterDatabase.");
                return false;
            }

            if (Characters.Count == 0)
            {
                UnityEngine.Debug.LogWarning("Add at least one character in the CharacterDatabase.");
                return false;
            }

            if (Characters.Count > 0)
            {
                foreach (var stu in Characters)
                {
                    if (stu == null)
                    {
                        UnityEngine.Debug.LogWarning("Nullref in CharacterDatabase. Is an element empty?");

                        return false;
                    }
                }
            }

            return true;
        }
    }
}