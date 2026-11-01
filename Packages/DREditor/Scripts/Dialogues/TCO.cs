//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class TCO
    {
        public bool enabled;

        public float seatFocus;

        public float height;

        public float distance;


        public TCO()
        {

        }

        public TCO(float s, float h, float d)
        {
            seatFocus = s;
            height = h;
            distance = d;
        }

        public void Clear()
        {
            enabled = false;
            seatFocus = 0;
            height = 0;
            distance = 0;
        }

        public void SetEnabled(bool enabled) => this.enabled = enabled;

        public void SetSeat(float seatFocus) => this.seatFocus = seatFocus;

        public void SetHeight(float height) => this.height = height;

        public void SetDistance(float distance) => this.distance = distance;
    }
}