//Original Author: SeleniumSoul
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Characters
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Characters/Student", fileName = "New Student")]
    public class Student : Character
    {
        public StudentCard StudentCard;
    }
}

