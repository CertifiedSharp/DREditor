﻿using System.Collections.Generic;
using UnityEngine;

namespace DREditor.CharacterEditor
{
    [System.Serializable]
    public class Character : ScriptableObject
    {
        public string LastName = "";
        public string FirstName = "";
        public List<Expression> Expressions = new List<Expression>();
        public List<string> Aliases = new List<string>();
    }
}
