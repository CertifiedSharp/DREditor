//Backlog Scriptable Object script for DREditor Author: SeleniumSoul
// Edited by Benjamin "Sweden" Jillson : Discord: sweden_gaming to use FMOD
using System.Collections.Generic;
using UnityEngine;
using DREditor.Characters;
using System;

namespace DREditor.Dialogues
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Dialogues/Backlog File", fileName = "New Backlog File")]
    public class Backlog : ScriptableObject
    {
        [Tooltip("")]
        public int Capacity = 50;

        public Queue<BacklogLines> Lines = new();

        public virtual BacklogLines[] GetLinesArray() => Lines.ToArray();

        public void AddLine(Character charfile, string text, int aliasNum, Color color, string voice = null)
        {
            BacklogLines _backline = new(
                charfile,
                text,
                aliasNum,
                voice,
                color
                );


            if (Lines.Count < Capacity)
            {
                Lines.Enqueue(_backline);
            }
            else
            {
                Lines.Enqueue(_backline);
                Lines.Dequeue();
            }
        }

        public BacklogData Save()
        {
            BacklogData data = new();

            data.SetLines(Lines.ToArray());

            return data;
        }

        public virtual void CreateDebugBacklogLines()
        {
            Lines.Clear();

            for (int i = 0; i < Capacity; i++)
            {
                BacklogLines line = new ("Tutorial", "Backlog Line: " + i);

                Lines.Enqueue(line);
            }
        }

        public void Load(BacklogData data)
        {
            try
            {
                Lines.Clear();

                foreach (BacklogLines l in data.Lines)
                {
                    Lines.Enqueue(l);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning("WARNING!: The Game could not load Backlog data due to: " + e.ToString());
            }
        }
    }

    [System.Serializable]
    public class BacklogData
    {
        public BacklogLines[] Lines;

        public void SetLines(BacklogLines[] Lines) => this.Lines = Lines;
    }

    [System.Serializable]
    public class BacklogLines
    {
        public string Text;
        public string CharFirstName;
        public int AliasNum = -1;
        public Character Charfile;
        public string Voice;
        public Color color = Color.white;

        public Color Color { get => color; set => color = value; }

        public BacklogLines() { }

        public BacklogLines(string CharFirstName, string Text)
        {
            this.CharFirstName = CharFirstName;

            this.Text = Text;
        }

        public BacklogLines(Character Charfile, string text, 
            int AliasNum, string Voice, Color color) : this(Charfile != null ? Charfile.FirstName : "", text)
        {
            this.AliasNum = AliasNum;
            this.Charfile = Charfile;
            this.Voice = Voice;
            this.color = color;
        }
    }
}