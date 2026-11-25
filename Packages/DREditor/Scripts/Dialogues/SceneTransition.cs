//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public class SceneTransition : EndEventBase
    {
        [FormerlySerializedAs("Scene")]
        public string scene;

        public string Scene { get => scene; set => scene = value; }


        [FormerlySerializedAs("ToDark")]
        public bool toDark;

        public bool ToDark { get => toDark; set => toDark = value; }


        [FormerlySerializedAs("ToMenu")]
        public bool toMenu;

        public bool ToMenu { get => toMenu; set => toMenu = value; }


        [FormerlySerializedAs("AtEnd")]
        public ScriptableObject atEnd;

        public ScriptableObject AtEnd { get => atEnd; set => atEnd = value; }


        [FormerlySerializedAs("OnLoadNoDark")]
        public bool onLoadNoDark;

        public bool OnLoadNoDark { get => onLoadNoDark; set => onLoadNoDark = value; }


        [FormerlySerializedAs("MoveToObjective")]
        public string moveToObjective;

        public string MoveToObjective { get => moveToObjective; set => moveToObjective = value; }


        [FormerlySerializedAs("MoveToNextNaturalObjective")]
        public bool moveToNextNaturalObjective;

        public bool MoveToNextNaturalObjective { get => moveToNextNaturalObjective; set => moveToNextNaturalObjective = value; }
    }
}