//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.Saveables
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public interface ISaveManager
    {
        /// <summary>
        ///
        /// </summary>
        int SubID { get; }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string Save();

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        void Load(string data);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        List<string> GetPreviousNames();
    }
}
// NOTE: Another issue with the Savables structure is that this structure is harder to extend and may not be super useful.
// NOTE: As of writing this the only topics that use this are The UISpriteCharacterSwapper and Maps data.
// Honestly, this system should be removed. Updating how maps are saved should be investigated.
// NOTE: Issue with the Save Manager structure: If you rename something after a save files been made 
// the game won't be able to load that data because the save is by the name of the class. 
/* The ISaveManager is designed to make managers to dynamically save data by transfering it to data strings and 
 * being able to load it whenever one needs it etc.
public class Manager : ISaveManager
{

    public int SubID { get => subID; }
    [SerializeField] int subID;
    public string Save()
    {
        return JsonUtility.ToJson(InstanceData);
    }

    public void Load(string data)
    {
        InstanceData = JsonUtility.FromJson<MapData>(data);
    }
}
*/