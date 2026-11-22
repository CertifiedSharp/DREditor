//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEditor;

namespace DREditor.Editor.Code
{
    /// <summary>
    ///
    /// </summary>
    public class XmlDocGenerator : AssetPostprocessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string OnGeneratedCSProject(string path, string content)
        {
            if (!content.Contains("<GenerateDocumentationFile>"))
            {
                string target = "<PropertyGroup>";

                string replacement = "<PropertyGroup>\n    <GenerateDocumentationFile>true</GenerateDocumentationFile>";

                if (!CodeSettings.Instance.EnforceXMLDocumentation)
                    replacement += "\n    <NoWarn>$(NoWarn);1591</NoWarn>";

                content = content.Replace(target, replacement);
            }

            return content;
        }
    }
}