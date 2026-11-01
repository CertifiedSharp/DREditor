//Original Author: Happy30
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Characters
{
    [System.Serializable]
    public class StudentCard
    {
        [FormerlySerializedAs("Talent")]
        public string talent;
        public string Talent { get => talent; set => talent = value; }

        [FormerlySerializedAs("Height")]
        public string height;
        public string Height { get => height; set => height = value; }

        [FormerlySerializedAs("Weight")]
        public string weight;
        public string Weight { get => weight; set => weight = value; }

        [FormerlySerializedAs("Chest")]
        public string chest;
        public string Chest { get => chest; set => chest = value; }

        [FormerlySerializedAs("BloodType")]
        public string bloodType;
        public string BloodType { get => bloodType; set => bloodType = value; }

        [FormerlySerializedAs("DateOfBirth")]
        public string dateOfBirth;
        public string DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }

        [FormerlySerializedAs("Likes")]
        public string likes;
        public string Likes { get => likes; set => likes = value; }

        [FormerlySerializedAs("Dislikes")]
        public string dislikes;
        public string Dislikes { get => dislikes; set => dislikes = value; }

        [FormerlySerializedAs("Notes")]
        public string notes;
        public string Notes { get => notes; set => notes = value; }

        [FormerlySerializedAs("Color")]
        public Color color = new(1, 1, 1, 1);
        public Color Color { get => color; set => color = value; }

        [FormerlySerializedAs("Age")]
        public string age;
        public string Age { get => age; set => age = value; }

        [FormerlySerializedAs("Picture")]
        public Texture2D picture;
        public Texture2D Picture { get => picture; set => picture = value; }

        [FormerlySerializedAs("Signature")]
        public Texture2D signature;
        public Texture2D Signature { get => signature; set => signature = value; }
    }
}

