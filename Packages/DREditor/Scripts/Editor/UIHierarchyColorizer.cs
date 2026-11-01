//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class UIHierarchyColorizer
    {
        #region Fields

        #region Runtime

        /// <summary>
        ///
        /// </summary>
        private GameObject firstFullMatch;

        /// <summary>
        ///
        /// </summary>
        private float hue;

        /// <summary>
        ///
        /// </summary>
        private int fullScreenDims = 0;

        /// <summary>
        ///
        /// </summary>
        private int overlayCount = 0;

        #endregion

        #region Coloring

        /// <summary>
        ///
        /// </summary>
        private readonly System.Type[] targetTypes = { typeof(Image), typeof(RawImage), typeof(Text) };

        /// <summary>
        ///
        /// </summary>
        private const float GoldenRatioConjugate = 0.61803398875f;

        /// <summary>
        ///
        /// </summary>
        private const float ShadeSatMin = 0.30f;

        /// <summary>
        ///
        /// </summary>
        private const float ShadeSatMax = 1.00f;

        /// <summary>
        ///
        /// </summary>
        private const float ShadeValMin = 0.50f;

        /// <summary>
        ///
        /// </summary>
        private const float ShadeValMax = 1.00f;

        /// <summary>
        ///
        /// </summary>
        private const float ComponentAlpha = 1.00f;

        #endregion

        #region Full Screen Check

        /// <summary>
        ///
        /// </summary>
        private enum ScreenSizeMatch { None, Partial, Full }

        /// <summary>
        ///
        /// </summary>
        private float RefScreenWidth = 1920f;

        /// <summary>
        ///
        /// </summary>
        private float RefScreenHeight = 1080f;

        /// <summary>
        ///
        /// </summary>
        private const float SizeTolerance = 0.5f;

        /// <summary>
        ///
        /// </summary>
        private const float FullScreenAlpha = 0.75f;

        #endregion

        #region Overlay Tinting Constants

        /// <summary>
        ///
        /// </summary>
        private const float OverlayHueShift = 0.5f;

        /// <summary>
        ///
        /// </summary>
        private const float OverlayAlpha = 0.50f;

        #endregion

        #region TMP Handling

        /// <summary>
        ///
        /// </summary>
        private bool s_tmpChecked;

        /// <summary>
        ///
        /// </summary>
        private System.Type s_tmpTextType;

        /// <summary>
        ///
        /// </summary>
        private System.Reflection.PropertyInfo s_tmpColorProp;

        /// <summary>
        ///
        /// </summary>
        private const string TMPTextTypeName = "TMPro.TMP_Text";

        /// <summary>
        ///
        /// </summary>
        private const string TMPTextColorPropertyName = "color";

        #endregion

        #endregion

        #region Colorize Hierarchy Groups

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/UI/Colorize Hierarchy Groups", priority = 100)]
        public static void ColorizeHierarchyGroups() => new UIHierarchyColorizer().Colorize();

        /// <summary>
        ///
        /// </summary>
        public void Colorize()
        {
            if (!ValidateSelection())
                return;

            InitializeResolution();

            DetectTextMeshPro();

            Undo.RegisterFullObjectHierarchyUndo(Selection.activeGameObject, "Colorize UI Hierarchy Groups");

            ColorizeHierarchy();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static bool ValidateSelection()
        {
            int groupCount = Selection.activeGameObject.transform.childCount;

            if (Selection.activeGameObject != null && groupCount != 0)
                return true;

            if (groupCount == 0)
                EditorUtility.DisplayDialog("No Children",
                    "REPORT: The selected GameObject has no children to colorize." +
                    "\n\nPROPOSAL: Select a game object with children containing any of the components: " +
                    "\nImage, Raw Image, or TMP Text.", "OK");
            else
                EditorUtility.DisplayDialog("No Selection",
                "PROPOSAL: Please select a parent GameObject in the Hierarchy first.", "OK");

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        private void InitializeResolution()
        {
            if (Selection.activeGameObject.TryGetComponent(out CanvasScaler scaler))
            {
                RefScreenWidth = scaler.referenceResolution.x;

                RefScreenHeight = scaler.referenceResolution.y;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void DetectTextMeshPro()
        {
            if (!s_tmpChecked)
            {
                s_tmpChecked = true;

                foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    s_tmpTextType = asm.GetType(TMPTextTypeName);

                    if (s_tmpTextType != null)
                        break;
                }

                if (s_tmpTextType != null)
                    s_tmpColorProp = s_tmpTextType.GetProperty(TMPTextColorPropertyName);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="matchingChild"></param>
        /// <returns></returns>
        private GameObject FindFirstFullMatch(GameObject go, bool matchingChild = false)
        {
            if (matchingChild && GetScreenSizeMatch(go) == ScreenSizeMatch.Full)
                return go;

            foreach (Transform child in go.transform)
                if (FindFirstFullMatch(child.gameObject, true) is GameObject found)
                    return found;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        private ScreenSizeMatch GetScreenSizeMatch(GameObject go)
        {
            if (!go.TryGetComponent<RectTransform>(out var rectTransform))
                return ScreenSizeMatch.None;

            float width = rectTransform.rect.width;

            float height = rectTransform.rect.height;

            bool widthMatch = Mathf.Abs(width - RefScreenWidth) <= SizeTolerance ||
                Mathf.Abs(width - RefScreenHeight) <= SizeTolerance;

            bool heightMatch = Mathf.Abs(height - RefScreenHeight) <= SizeTolerance ||
                Mathf.Abs(height - RefScreenWidth) <= SizeTolerance;

            if (widthMatch && heightMatch)
                return ScreenSizeMatch.Full;

            if (widthMatch || heightMatch)
                return ScreenSizeMatch.Partial;

            return ScreenSizeMatch.None;
        }

        /// <summary>
        ///
        /// </summary>
        private void ColorizeHierarchy()
        {
            firstFullMatch = FindFirstFullMatch(Selection.activeGameObject);

            hue = Random.value;

            fullScreenDims = 0;

            overlayCount = 0;

            int totalColored = 0;

            int groupCount = Selection.activeGameObject.transform.childCount;

            for (int i = 0; i < groupCount; i++)
            {
                Transform group = Selection.activeGameObject.transform.GetChild(i);

                // Flatten the subtree depth-first so every object gets a unique index.
                List<GameObject> objects = new();

                CollectAllDescendants(group.gameObject, objects);

                ColorizeObjects(objects);

                totalColored += objects.Count;

                hue = (Random.value + GoldenRatioConjugate) % 1f;
            }

            UnityEngine.Debug.Log($"REPORT: {nameof(UIHierarchyColorizer)}: Colorized {totalColored} object(s) across " +
                      $"{groupCount} group(s) under \"{Selection.activeGameObject.name}\". " +
                      $"Full-screen dimmed: {fullScreenDims}  |  Overlay contrasted: {overlayCount}  |  " +
                      $"First full-screen object preserved: {(firstFullMatch != null ? firstFullMatch.name : "none")}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="root"></param>
        /// <param name="result"></param>
        private void CollectAllDescendants(GameObject root, List<GameObject> result)
        {
            result.Add(root);

            foreach (Transform child in root.transform)
                CollectAllDescendants(child.gameObject, result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objects"></param>
        private void ColorizeObjects(List<GameObject> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                GameObject go = objects[i];

                Color shade = CreateObjectColor(go, i, objects.Count);

                ApplyColorToComponents(go, shade);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="i"></param>
        /// <param name="objectCount"></param>
        /// <returns></returns>
        private Color CreateObjectColor(GameObject go, int i, int objectCount)
        {
            float t = objectCount > 1 ? (float)i / (objectCount - 1) : 0.5f;

            float sat = Mathf.Lerp(ShadeSatMin, ShadeSatMax, t);

            float val = Mathf.Lerp(ShadeValMax, ShadeValMin, t);

            ScreenSizeMatch match = GetScreenSizeMatch(go);

            float shadeHue = match != ScreenSizeMatch.Partial ? hue : (hue + OverlayHueShift) % 1f;

            Color shade = Color.HSVToRGB(shadeHue, sat, val);

            if (match == ScreenSizeMatch.Full)
            {
                bool firstMatch = go == firstFullMatch;

                shade.a = firstMatch ? ComponentAlpha : FullScreenAlpha;

                if (!firstMatch)
                    fullScreenDims++;
            }
            else if (match == ScreenSizeMatch.Partial)
            {
                shade.a = OverlayAlpha;

                overlayCount++;
            }
            else
                shade.a = ComponentAlpha;

            return shade;
        }

        #endregion

        #region Reset Hierarchy Colors

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/UI/Reset Hierarchy Colors", priority = 101)]
        public static void ResetHierarchyColors() => new UIHierarchyColorizer().ResetSelection();

        /// <summary>
        ///
        /// </summary>
        public void ResetSelection()
        {
            if (!ValidateSelection())
                return;

            GameObject root = Selection.activeGameObject;

            Undo.RegisterFullObjectHierarchyUndo(root, "Reset UI Hierarchy Colors");

            ResetColorRecursive(root);

            UnityEngine.Debug.Log($"REPORT: {nameof(UIHierarchyColorizer)}: " +
                $"Reset all UI component colors under \"{root.name}\" to white.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        private void ResetColorRecursive(GameObject go)
        {
            ApplyColorToComponents(go, Color.white);

            foreach (Transform child in go.transform)
                ResetColorRecursive(child.gameObject);
        }

        #endregion

        #region Apply Color To Components

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="color"></param>
        private void ApplyColorToComponents(GameObject go, Color color)
        {
            ColorGraphics(go, color);

            DetectTextMeshPro();

            AttemptColorTMP(go, Random.ColorHSV());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="color"></param>
        private void ColorGraphics(GameObject go, Color color)
        {
            foreach (var type in targetTypes)
            {
                if (go.TryGetComponent(type, out var component))
                {
                    var graphic = (Graphic)component;

                    Undo.RecordObject(graphic, $"Color {type.Name}");

                    if (color.a == ComponentAlpha)
                        color.a = graphic.color.a;

                    graphic.color = color;
                    
                    EditorUtility.SetDirty(graphic);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="color"></param>
        private void AttemptColorTMP(GameObject go, Color color)
        {
            if (s_tmpChecked && s_tmpTextType == null)
                return;

            if (s_tmpTextType == null || s_tmpColorProp == null)
                return;

            var tmp = go.GetComponent(s_tmpTextType);

            if (tmp == null)
                return;

            Undo.RecordObject(tmp, "Color TMP_Text");

            if (color.a == ComponentAlpha)
                color.a = ((Color)s_tmpColorProp.GetValue(tmp)).a;

            s_tmpColorProp.SetValue(tmp, color);

            EditorUtility.SetDirty(tmp);
        }

        #endregion
    }
}
#endif