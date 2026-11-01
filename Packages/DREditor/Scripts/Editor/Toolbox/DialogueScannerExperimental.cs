//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace DREditor.Editor.Toolbox
{
    public class DialogueScannerExperimental
    {
        #region Dialogue Variables Scanner
        /*
         * This region of code is an incomplete scanner, meant to be able to filter and display the names of
         * dialogue objects that contained the given parameters.
         */
        List<Type> oneTypes = new List<Type>();
        //List<int> idxs = new List<int>();
        int idx = -1;
        int vart = 0;
        public void ScanGUI()
        {

            using (new EditorGUILayout.HorizontalScope())
            {
                if (oneTypes.Count == 0)
                    oneTypes = GetInherited(typeof(DialogueCore));
                else
                {
                    idx = EditorGUILayout.Popup(idx, GetTypeNames(oneTypes).ToArray());
                }

                if (idx != -1)
                {
                    vart = EditorGUILayout.Popup(vart, GetPublicPropertyNames(oneTypes[idx]).ToArray());
                }

            }
        }
        public static List<Type> GetInherited(Type baseType)
        {
            List<Type> types = new();

            foreach (var x in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in FindDerivedTypes(x, baseType))
                {
                    types.Add(t);
                }
            }
            return types;
        }
        public static List<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t)).ToList();
        }
        public static List<string> GetPublicPropertyNames(Type type)
        {
            List<string> propertyNames = new List<string>();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                propertyNames.Add(property.Name);
            }
            return type.GetFields()
                            .Select(field => field.Name)
                            .ToList(); ;
        }
        public static List<Type> GetPublicAndProtectedVariables(Type type)
        {
            List<Type> variableNames = new List<Type>();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var field in fields)
            {
                variableNames.Add(field.GetType());
            }
            return variableNames;
        }
        public static List<string> GetTypeNames(List<Type> types)
        {
            return types.Select(t => t.Name).ToList();
        }
        #endregion
    }
}
#endif



//void FindIt()
//{
//    var assets = FindAssetsByType<Dialogue>();
//    for (int j = 0; j < assets.Count; j++)
//    {
//        Dialogue dia = assets[j];
//        if (dia.Lines[0].Speaker != null && dia.Lines[0].Speaker.FirstName.Contains("Damon"))
//        {
//            Debug.Log("Asset Found Called: " + dia.name + " on line: " + 0);
//        }
//        for (int x = 0; x < dia.Lines.Count; x++)
//        {
//            Line currentLine = dia.Lines[x];
//            /* Dialogues with Last Actor Event
//            var vid = currentLine.DiaEvents.Where(n => n.GetType() == typeof(LastActorSprite));
//            if (vid.Count() > 0)
//            {
//                LastActorSprite v = (LastActorSprite)vid.ElementAt(0);
//                if (v != null)
//                {
//                    Debug.Log("Asset Found Called: " + dia.name + " on line: " + x);
//                }
//            }
//            */
//            /*
//            var vid = currentLine.DiaEvents.Where(n => n.GetType() == typeof(VideoDisplay));
//            if (vid.Count() > 0)
//            {
//                VideoDisplay v = (VideoDisplay)vid.ElementAt(0);
//                if (v.waitToEnd)
//                {
//                    Debug.Log("Asset Found Called: " + dia.name + " on line: " + x);
//                }
//            }
//            */

//        }
//    }
//    Close();
//}