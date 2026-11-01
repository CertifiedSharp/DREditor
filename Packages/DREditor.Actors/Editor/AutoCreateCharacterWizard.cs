//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Actors.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class AutoCreateCharacterWizard : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string targetDirectory = "Resources/Characters";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string firstName = string.Empty;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string lastName = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color color = Color.white;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite neutralSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Texture2D nameplate = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Shader shader;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Shader blackShader;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject actorPrefab = null;


        /// <summary>
        ///
        /// </summary>
        protected Material blackMaterial = null;


        [MenuItem("DREditor/Characters/Auto Setup Character")]
        public static void ShowWizard() => DisplayWizard<AutoCreateCharacterWizard>("Auto Setup Character", "Create");

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            shader = Shader.Find("Universal Render Pipeline/Lit");

            blackShader = Shader.Find("Shader Graphs/BlackCutOut");

            actorPrefab = BuilderUtility.FindAssetByType<GameObject>("Actor");

            nameplate = BuilderUtility.FindAssetByType<Texture2D>("Transparent Image");

            characterDatabase = DatabaseUtility.LoadDatabase<CharacterDatabase>();

            if (Selection.objects[0] != null)
                neutralSprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(Selection.objects[0]));
        }


        protected virtual void OnWizardCreate()
        {
            using (new AssetDatabase.AssetEditingScope())
                AutoCreateCharacter();

            EditorApplication.ExecuteMenuItem("DREditor/Characters/Create Dialogue With All Sprites");
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void AutoCreateCharacter()
        {
            try
            {
                Student student = CreateInstance<Student>();

                CreateStudentAsset(student);

                var materials = CreateMaterials(shader);

                CreateActorPrefab(materials);

                ImportExpressions(student, materials);

                ImportSprites(student);

                Selection.activeObject = student;

                EditorApplication.ExecuteMenuItem("DREditor/Characters/Sort Expressions by their numbers");
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogException(exception);
            }

            UnityEngine.Debug.Log("REPORT: Auto Creation Process Finished.");
        }

        #region Create Student Asset

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        protected virtual void CreateStudentAsset(Character character)
        {
            if (firstName != "")
            {
                SpawnStudentAsset(character);

                FillOutStudentData(character);

                AddCharacterToDatabase(character);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        protected virtual void SpawnStudentAsset(Character character)
        {
            string folder = FolderUtility.CreateIntermediateFolders(targetDirectory);

            string stuPath = folder + "/" + firstName + ".asset";

            AssetDatabase.CreateAsset(character, stuPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        protected virtual void FillOutStudentData(Character character)
        {
            character.FirstName = firstName;

            character.LastName = lastName;

            character.Nameplate = nameplate;

            if (character is Student student)
                student.StudentCard.Color = color;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        protected virtual void AddCharacterToDatabase(Character character)
        {
            if (characterDatabase == null && BuilderUtility.FindAssetByType<CharacterDatabase>() == null)
            {
                string folder = FolderUtility.CreateIntermediateFolders(targetDirectory);

                CharacterDatabase characterDatabase = CreateInstance<CharacterDatabase>();

                string stuPath = folder + "/CharacterDatabase.asset";

                AssetDatabase.CreateAsset(characterDatabase, stuPath);

                AssetDatabase.SaveAssets();

                this.characterDatabase = characterDatabase;
            }

            characterDatabase.Characters.Add(character);

            EditorUtility.SetDirty(characterDatabase);
        }

        #endregion

        #region Create Materials

        /// <summary>
        ///
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="spriteList"></param>
        /// <returns></returns>
        protected virtual List<Material> CreateMaterials(Shader shader, List<Texture> spriteList = null)
        {
            List<Material> materials = new();

            try
            {
                IEnumerable<Texture> textures = spriteList
                    ?? Selection.GetFiltered(typeof(Texture), SelectionMode.Assets).Cast<Texture>();

                foreach (Texture texture in textures)
                    if (CreateMaterialFromTexture(texture, shader) is Material material)
                        materials.Add(material);
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogWarning("ALERT: Something went wrong creating materials.");

                UnityEngine.Debug.LogException(exception);
            }

            return materials;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="shader"></param>
        /// <returns></returns>
        protected virtual Material CreateMaterialFromTexture(Texture texture, Shader shader)
        {
            Material material = new(shader)
            {
                mainTexture = texture
            };

            string path = PopulateShadersAndMaterialPath(material, texture, shader);

            if (string.IsNullOrEmpty(path))
                return null;
            else
                AssetDatabase.CreateAsset(material, path);

            return material;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="material"></param>
        /// <param name="texture"></param>
        /// <param name="shader"></param>
        /// <returns></returns>
        protected virtual string PopulateShadersAndMaterialPath(Material material, Texture texture, Shader shader)
        {
            string path = AssetDatabase.GetAssetPath(texture);

            path = path[..path.LastIndexOf(".")] + ".mat";

            if (shader.name == "Universal Render Pipeline/Lit")
            {
                material.SetFloat("_Smoothness", 0);

                material.SetFloat("_AlphaClip", 1);

                material.SetFloat("_Cutoff", 0.5f);

                material.EnableKeyword("_ALPHATEST_ON");
            }

            if (shader.name == "Shader Graphs/BlackCutOut")
            {
                material.SetColor("_Color", new Color(0, 0, 0));

                path = path.Replace(texture.name, texture.name + "b");

                material.name += "b";
            }

            if (AssetDatabase.LoadAssetAtPath(path, typeof(Material)) != null)
            {
                UnityEngine.Debug.LogWarning("ALERT: Can't create material, it already exists: " + path);

                return "";
            }

            return path;
        }

        #endregion

        #region Create Actor Prefab

        /// <summary>
        ///
        /// </summary>
        /// <param name="materials"></param>
        protected virtual void CreateActorPrefab(List<Material> materials)
        {
            if (neutralSprite != null)
            {
                int i = 0;

                foreach (Material material in materials)
                    if (material.name == neutralSprite.name)
                        i = materials.IndexOf(material);

                Material neutralMat = materials[i];

                materials.Remove(neutralMat);

                materials.Insert(0, neutralMat);

                blackMaterial = CreateMaterials(blackShader, new List<Texture>() { materials[0].mainTexture })[0];

                if (actorPrefab != null)
                    SpawnActorPrefab(materials);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="materials"></param>
        protected virtual void SpawnActorPrefab(List<Material> materials)
        {
            GameObject actorPrefabInstance = PrefabUtility.InstantiatePrefab(actorPrefab) as GameObject;

            if (actorPrefabInstance.TryGetComponent<Actor>(out var actor))
            {
                actor.DisplayName = firstName + " " + lastName;

                var meshes = actor.GetComponentsInChildren<MeshRenderer>();

                var sprites = actor.GetComponentsInChildren<SpriteRenderer>();

                actorPrefabInstance.name = firstName;

                meshes[0].material = materials[0];

                meshes[1].material = blackMaterial;

                sprites[0].sprite = neutralSprite;

                sprites[1].sprite = neutralSprite;

                sprites[0].transform.position = new(0, 1.5f, 0);

                actorPrefabInstance.transform.position = new(0, 0.27f, 0);

                EditorUtility.SetDirty(actor);

                EditorUtility.SetDirty(actorPrefabInstance);
            }
        }

        #endregion

        #region Import Expressions and Sprites

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="materials"></param>
        protected virtual void ImportExpressions(Character character, List<Material> materials = null)
        {
            if (firstName == "")
                return;

            var selectedMaterials = materials
                ?? Selection.GetFiltered(typeof(Material), SelectionMode.Assets).Cast<Material>().ToList();

            UnityEngine.Debug.Log("REPORT: Imported material count is: " + selectedMaterials.Count);

            foreach (var material in selectedMaterials)
            {
                if (material == null)
                    continue;

                string expressionName = material.name
                    .Replace("_", "")
                    .Replace(" ", "")
                    .Replace("-", "")
                    .Replace(firstName, "");

                Expression expression = new(expressionName, material);

                character.Expressions.Add(expression);

                character.Sprites.Add(new Unlit(expression.Name));
            }

            character.BlackExpression = blackMaterial;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="sprites"></param>
        protected virtual void ImportSprites(Character character, List<Sprite> sprites = null)
        {
            if (firstName == "")
                return;

            List<Texture2D> selectedTextures = sprites?.Cast<Texture2D>().ToList()
                ?? Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets).Cast<Texture2D>().ToList();

            UnityEngine.Debug.Log("REPORT: Imported texture count is: " + selectedTextures.Count);

            SetCharacterSprites(character, selectedTextures);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="selectedTextures"></param>
        protected virtual void SetCharacterSprites(Character character, List<Texture2D> selectedTextures)
        {
            List<Sprite> selectedSprites = new();

            for (int i = 0; i < selectedTextures.Count; i++)
                selectedSprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(selectedTextures[i])));

            for (int i = 0; i < character.Sprites.Count; i++)
            {
                for (int x = 0; x < selectedSprites.Count; x++)
                {
                    if (character.Expressions[i].Sprite.name == selectedSprites[x].name)
                    {
                        character.Sprites[i].SetSprite(selectedSprites[x]);

                        UnityEngine.Debug.Log("REPORT: Successfully Loaded " + selectedSprites[x].name);

                        break;
                    }
                }
            }
        }

        #endregion
    }
}