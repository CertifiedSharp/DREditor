using UnityEditor;

namespace DREditor.Editor
{
    /// <summary>
    /// This class is so that XML documentation can be generated when visual studio builds.
    /// </summary>
    public class XmlDocGenerator : AssetPostprocessor
    {
        private static string OnGeneratedCSProject(string path, string content)
        {
            if (!content.Contains("<GenerateDocumentationFile>"))
            {
                string target = "<PropertyGroup>";

                string replacement = "<PropertyGroup>\n    <GenerateDocumentationFile>true</GenerateDocumentationFile>";

                content = content.Replace(target, replacement);
            }

            return content;
        }
    }
}