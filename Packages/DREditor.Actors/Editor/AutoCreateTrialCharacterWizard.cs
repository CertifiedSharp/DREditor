//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Characters;
using DREditor.Editor.Utility;
using DREditor.Talkables;
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
    public class AutoCreateTrialCharacterWizard : ScriptableWizard
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;


        /// <summary>
        /// 
        /// </summary>
        [Header("Trial Actor Settings")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string trialActorPrefabPath = "Assets/Prefabs/Actors/Trial";

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 prefabScale = new(3, 3, 3);


        /// <summary>
        /// 
        /// </summary>
        [Header("Trial Shader Settings")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Shader trialShader;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [ShowIf(nameof(UsingTrialShader))]
        [SerializeField] protected Vector2 extrudeSize = new(-1.01f, -1.01f);

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [ShowIf(nameof(UsingTrialShader))]
        [SerializeField] protected float outlineInfluence = 390f;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [ShowIf(nameof(UsingTrialShader))]
        [SerializeField] protected float lightAngle = -0.4f;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [ShowIf(nameof(UsingTrialShader))]
        [SerializeField] protected Color outlineColor = new(1, 1, 1, 0.02745098f);


        /// <summary>
        /// 
        /// </summary>
        [Header("Spawn Settings")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Transform trialCameraTransform;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform spawnParent;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [Range(0, 50)]
        [SerializeField] protected int numberOfObjects = 16;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float spawnRadius = 8.11f;

        /// <summary>
        /// 
        /// </summary>
        [Range(-360f, 360f)]
        [Tooltip("")]
        [SerializeField] protected float startingDegreeOffset = 90f;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Character> ignoredCharacters = new();


        /// <summary>
        /// 
        /// </summary>
        [Header("Editor Settings")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected AssignmentStyleGUIEditor<Character> assignmentEditor;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CircleDrawer circleDrawer = new();


        /// <summary>
        /// 
        /// </summary>
        protected bool changed;

        /// <summary>
        /// 
        /// </summary>
        protected int slot;

        /// <summary>
        /// 
        /// </summary>
        protected int newIndex;


        /// <summary>
        /// 
        /// </summary>
        protected readonly string spawnParentName = "Characters";


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MenuItem("DREditor/Characters/Auto Setup Trial Character", true)]
        public static bool VerifyWizard() =>
            DatabaseUtility.LoadDatabase<CharacterDatabase>() is CharacterDatabase database &&
            database.Characters.Count > 0;


        [MenuItem("DREditor/Characters/Auto Setup Trial Character")]
        public static void ShowWizard() => DisplayWizard<AutoCreateTrialCharacterWizard>("Auto Setup Trial Character", "Create");


        protected virtual void OnEnable()
        {
            minSize = new Vector2(1100, 800);

            characterDatabase = DatabaseUtility.LoadDatabase<CharacterDatabase>();

            trialShader = Shader.Find("Shader Graphs/CharacterTrial");

            if (FindAnyObjectByType<DRTrialCamera>() is DRTrialCamera camera)
                trialCameraTransform = camera.transform;

            if (GameObject.Find(spawnParentName) is GameObject spawnParent)
                this.spawnParent = spawnParent.transform;

            ResetAssignmentEditor();
        }

        protected virtual void OnWizardCreate()
        {
            DestroyAllChildren();

            SpawnTrialCharacters();

            EditorUtility.SetDirty(spawnParent);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DestroyAllChildren()
        {
            foreach (Transform child in spawnParent.transform)
                GameObject.DestroyImmediate(child.gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SpawnTrialCharacters()
        {
            for (int i = 0; i < assignmentEditor.Assignments.Length; i++)
                if (CharacterCanBeSpawnedAtAssignment(i))
                    SpawnTrialCharacter(assignmentEditor.Objects[assignmentEditor.Assignments[i]], i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool CharacterCanBeSpawnedAtAssignment(int i) =>
            assignmentEditor.Assignments[i] is int assignIndex &&
            assignIndex >= 0 &&
            assignIndex < assignmentEditor.Objects.Count &&
            assignmentEditor.Objects[assignIndex].ActorPrefab != null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="i"></param>
        protected virtual void SpawnTrialCharacter(Character character, int i)
        {
            float currentAngle = (startingDegreeOffset + CircleUtility.DegreesInCircle * -i / numberOfObjects) * Mathf.Deg2Rad;

            Vector3 spawnPosition = trialCameraTransform.position + 
                new Vector3(Mathf.Cos(currentAngle) * spawnRadius, 0f, Mathf.Sin(currentAngle) * spawnRadius);

            Vector3 directionFromCenter = (spawnPosition - trialCameraTransform.position).normalized;

            Quaternion spawnRotation = Quaternion.LookRotation(directionFromCenter);

            string path = AssetDatabase.GetAssetPath(character.ActorPrefab);

            GameObject actorPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            GameObject trialPrefab = SpawnObjectAt(actorPrefab, spawnPosition, spawnRotation);

            CustomEditorUtility.EnsureFolders(trialActorPrefabPath);

            PrefabUtility.SaveAsPrefabAsset(trialPrefab, trialActorPrefabPath + "/" + actorPrefab.name + ".prefab");

            character.TrialPosition = i;

            EditorUtility.SetDirty(character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefabToSpawn"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="spawnRotation"></param>
        /// <returns></returns>
        protected virtual GameObject SpawnObjectAt(GameObject prefabToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            GameObject spawned = (GameObject)PrefabUtility.InstantiatePrefab(prefabToSpawn, spawnParent);

            spawned.transform.SetPositionAndRotation(spawnPosition, spawnRotation);

            float xPos = spawned.transform.localPosition.x;

            float zPos = spawned.transform.localPosition.z;

            spawned.transform.localPosition = new(xPos, -1, zPos);

            float yRot = spawned.transform.eulerAngles.y;

            spawned.transform.localEulerAngles = new(0, yRot + 180, 0);

            spawned.transform.localScale = prefabScale;

            ApplyTrialShader(spawned);

            if (spawned.TryGetComponent(out SpriteLookAtTarget billboard))
                billboard.enabled = false;

            return spawned;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectToDuplicateName"></param>
        /// <param name="objectToDuplicatePath"></param>
        /// <returns></returns>
        protected virtual string BuildDuplicatedMaterialPath(string objectToDuplicateName, string objectToDuplicatePath) =>
            objectToDuplicatePath.Replace(objectToDuplicateName, objectToDuplicateName + "_Trial");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spawnedObj"></param>
        protected virtual void ApplyTrialShader(GameObject spawnedObj)
        {
            if (spawnedObj.transform.GetChild(0) is not Transform rendererChild || !rendererChild.TryGetComponent<MeshRenderer>(out var actorRenderer))
                return;

            Material baseMaterial = actorRenderer.sharedMaterial;

            if (CustomEditorUtility.TryDuplicateAsset(baseMaterial, BuildDuplicatedMaterialPath, out Material trialMaterial))
            {
                trialMaterial.shader = trialShader;

                actorRenderer.material = trialMaterial;

                if (UsingTrialShader())
                {
                    trialMaterial.SetVector(nameof(extrudeSize), extrudeSize);

                    trialMaterial.SetColor(nameof(outlineColor), outlineColor);

                    trialMaterial.SetFloat(nameof(outlineInfluence), outlineInfluence);

                    trialMaterial.SetFloat(nameof(lightAngle), lightAngle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool UsingTrialShader() => trialShader == Shader.Find("Shader Graphs/CharacterTrial");

        #region Editor GUI

        protected override bool DrawWizardGUI()
        {
            changed = base.DrawWizardGUI();

            HandySplitters.UISplitter();

            if (changed)
                ResetAssignmentEditor();
            else
                DrawCircleGUI();

            return changed;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ResetAssignmentEditor() => assignmentEditor = CreateNewAssignmentEditor();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual AssignmentStyleGUIEditor<Character> CreateNewAssignmentEditor() =>
            new(GetTrialCharacters(), GetCharacterName, numberOfObjects, assignmentEditor?.Assignments);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual List<Character> GetTrialCharacters()
        {
            if (ignoredCharacters.Count == 0)
                return characterDatabase.Characters;

            List<Character> characters = new();

            foreach (Character character in characterDatabase.Characters)
                if (ignoredCharacters.Contains(character))
                    continue;
                else
                    characters.Add(character);

            return characters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        protected virtual string GetCharacterName(Character character) => character.FirstName + " " + character.LastName;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DrawCircleGUI()
        {
            assignmentEditor?.ValidateAssignments();

            circleDrawer?.Draw();

            if (numberOfObjects == assignmentEditor.Assignments.Length)
                circleDrawer?.DrawObjectsAround(numberOfObjects, DrawDropdown, OnDropdownChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aroundCircleRect"></param>
        /// <param name="slot"></param>
        protected virtual void DrawDropdown(Rect aroundCircleRect, int slot)
        {
            this.slot = slot;

            newIndex = EditorGUI.Popup(aroundCircleRect, assignmentEditor.Assignments[slot], assignmentEditor.GetAssignedNames());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnDropdownChanged()
        {
            assignmentEditor.SwapAssignment(slot, newIndex);

            changed = true;
        }

        #endregion
    }
}