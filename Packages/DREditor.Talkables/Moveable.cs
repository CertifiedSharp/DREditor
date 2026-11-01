//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Talkables
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class Moveable : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [HideInInspector] protected MoveData place = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual MoveData Save()
        {
            place = new(gameObject.name, transform.position, transform.eulerAngles);

            return (MoveData)place.Clone();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="place"></param>
        public virtual void Load(MoveData place)
        {
            this.place = place;

            transform.position = place.position;

            transform.eulerAngles = place.rotation;
        }
    }
}