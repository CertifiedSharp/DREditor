//Original Author: Happy30
//Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Characters
{
    [System.Serializable]
    public class Expression
    {
        public Material Sprite;
        public string Name;

        public Expression() { }

        public Expression(string Name, Material Sprite)
        {
            this.Name = Name;

            this.Sprite = Sprite;
        }

        public void SetSprite(Material Sprite) => this.Sprite = Sprite;
        public void SetName(string Name) => this.Name = Name;
    }

    [System.Serializable] //*
    public class Unlit
    {
        public Unlit() { }

        public Unlit(string Name) => this.Name = Name;

        public Sprite Sprite;
        public string Name;
        public void SetSprite(Sprite Sprite) => this.Sprite = Sprite;
        public void SetName(string Name) => this.Name = Name;
    }
}