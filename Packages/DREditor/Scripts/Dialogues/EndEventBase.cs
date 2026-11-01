//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Dialogues
{
    public abstract class EndEventBase
    {
        public bool Enabled;

        public virtual void SetEnabled(bool Enabled) => this.Enabled = Enabled;
    }
}
