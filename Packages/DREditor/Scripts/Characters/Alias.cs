//Original Author: SeleniumSoul
//Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Characters
{
    [System.Serializable]
    public class Alias
    {
        [FormerlySerializedAs("Name")] 
        public string name = ""; 
        public string Name { get => name; set => name = value; }

        [FormerlySerializedAs("Nameplate")] 
        public Texture2D nameplate; 
        public Texture2D Nameplate { get => nameplate; set => nameplate = value; }

        [FormerlySerializedAs("TrialNameplate")] 
        public Texture2D trialNameplate; 
        public Texture2D TrialNameplate { get => trialNameplate; set => trialNameplate = value; }

        [FormerlySerializedAs("TrialPortrait")] 
        public Texture2D trialPortrait; 
        public Texture2D TrialPortrait { get => trialPortrait; set => trialPortrait = value; }
    }
}
