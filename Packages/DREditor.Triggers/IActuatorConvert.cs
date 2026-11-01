//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    public interface IActuatorConvert
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="genericActuator"></param>
        /// <param name="oldActuator"></param>
        void Convert(ref GenericActuator genericActuator, GenericActuator oldActuator);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericActuator"></param>
        /// <returns></returns>
        bool CanConvert(GenericActuator genericActuator);
    }
}