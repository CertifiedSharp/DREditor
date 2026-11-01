//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        public static object GetTargetObjectOfProperty(this SerializedProperty prop)
        {
            object target = prop.serializedObject.targetObject;

            string[] path = prop.propertyPath.Replace(".Array.data[", "[").Split('.');

            foreach (var part in path)
            {
                if (part.Contains("["))
                {
                    string elementName = part[..part.IndexOf("[")];

                    int index = int.Parse(part[part.IndexOf("[")..].Replace("[", "").Replace("]", ""));

                    target = GetFieldValue(target, elementName);

                    target = ((System.Collections.IEnumerable)target).Cast<object>().ElementAt(index);
                }
                else
                    target = GetFieldValue(target, part);
            }

            return target;
        }

        private static object GetFieldValue(object obj, string name)
        {
            if (obj == null)
                return null;

            var type = obj.GetType();

            FieldInfo field = null;

            while (type != null && field == null)
            {
                field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                type = type.BaseType;
            }

            return field?.GetValue(obj);
        }
    }
}
#endif