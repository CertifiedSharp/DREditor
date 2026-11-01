//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.IO;
using UnityEngine;
namespace DREditor.Editor.Utility
{
    public static class ResourcesExtension
    {
        public static T Load<T>(string resourceName) where T : UnityEngine.Object
        {
            string[] directories = Directory.GetDirectories(Application.dataPath + "/Resources", "*", SearchOption.AllDirectories);

            foreach (var item in directories)
            {
                string itemPath = item.Substring((Application.dataPath + "/Resources").Length + 1);

                var result = Resources.Load<T>(itemPath + "\\" + resourceName);

                if (result != null) 
                    return result;
            }

            return null;
        }
    }
}