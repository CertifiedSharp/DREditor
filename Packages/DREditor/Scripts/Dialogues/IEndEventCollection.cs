//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;

namespace DREditor.Dialogues
{
    /* ALERT: The following should be applied to this script after DREditor Ver.0.1.12 :
     * Insead of EndEventBase investigate just using IEndEvent.
     * Investigate moving this script to DREditor.EndEvents.
     * Remember to apply the MovedFrom Attribute properly if this script is moved.
     * If this is moved, also make sure that the Dialogue Event converter tool converts this stuff properly.
    */
    /// <summary>
    /// Interface for processes that contain a list of <see cref="IEndEvent"/>.
    /// </summary>
    public interface IEndEventCollection
    {
        /// <summary>
        /// The Collection of <see cref="IEndEvent"/>s held by the implementer.
        /// </summary>
        List<IEndEvent> EndEvents { get; }

        /// <summary>
        /// Retrieves the first found <see cref="IEndEvent"/> held by this container.
        /// </summary>
        /// <typeparam name="T">A type inheriting <see cref="IEndEvent"/>.</typeparam>
        /// <returns>The first available <see cref="IEndEvent"/> within the collection.</returns>
        T GetEndEvent<T>();
    }
}
