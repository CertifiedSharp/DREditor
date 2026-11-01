//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Dialogues
{
    [System.Serializable]
    public class TCOPreset
    {
        public string presetName;

        public TCO Preset;


        public TCOPreset(string name, TCO preset)
        {
            presetName = name;

            Preset = preset;
        }
    }
}