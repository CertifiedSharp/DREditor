//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Actors;
using DREditor.Characters;
using DREditor.LocalDialogues;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation.Actors
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomStateHandlerBase<ActorStateData, ActorData>) + 
        " for types <" + nameof(ActorStateData) + ", " + nameof(ActorData) + ">" +
        "\n\n")]
    [ExecuteInEditMode]
    public class ActorStateLoader : RoomStateHandlerBase<ActorStateData, ActorData>
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
        [SerializeField] protected List<GameObject> ActorPrefabs = new();


        protected virtual void OnValidate() => PrefabReferences();

        /// <summary>
        ///
        /// </summary>
        protected virtual void PrefabReferences()
        {
            if (characterDatabase != null && ActorPrefabs.Count < characterDatabase.GetActorPrefabCount())
            {
                List<GameObject> actorPrefabs = new();

                foreach (Character character in characterDatabase.Characters)
                    if (character.ActorPrefab != null)
                        actorPrefabs.Add(character.ActorPrefab);

                ActorPrefabs = actorPrefabs;

                UnityEngine.Debug.Log(nameof(ActorStateLoader) + ": Updated actor prefab references.");
            }
        }

        /// <inheritdoc/>
        public override List<ActorData> Save()
        {
            Actor[] actors = (Actor[])FindObjectsByType(typeof(Actor));

            if (actors.Length == 0)
                return new();

            List<ActorData> actorDataInstances = new();

            for (int i = 0; i < actors.Length; i++)
                for (int j = 0; j < ActorPrefabs.Count; j++)
                    if (ActorPrefabs[j] != null && ActorPrefabs[j].name == actors[i].gameObject.name)
                        actorDataInstances.Add((ActorData)new ActorData(ActorPrefabs[j].name, actors[i]).Clone());

            return actorDataInstances;
        }


        /// <inheritdoc/>
        public override void Unload(bool inEditor)
        {
            Actor[] actors = (Actor[])FindObjectsByType(typeof(Actor));

            if (actors.Length == 0)
                return;

            for (int i = 0; i < actors.Length; i++)
                for (int j = 0; j < ActorPrefabs.Count; j++) // Look through Actor references
                    if (ActorPrefabs[j] != null && 
                        ActorPrefabs[j].name == actors[i].gameObject.name) // Found
                    {
                        if (inEditor)
                            DestroyImmediate(actors[i].gameObject);
                        else
                            Destroy(actors[i].gameObject);

                        break;
                    }
        }

        /// <inheritdoc/>
        protected override bool LoadedInstanceValid(string dataInstance, out ActorData result) 
            => base.LoadedInstanceValid(dataInstance, out result) && !string.IsNullOrEmpty(result.aReference);

        /// <inheritdoc/>
        protected override void LoadAssetProcess(List<ActorData> baseDatas, List<ActorData> instanceDatas = null) 
            => LoadActors(baseDatas, instanceDatas);

        /// <inheritdoc/>
        protected override void InitializeVisibleBaseDatas(List<ActorData> baseDatas)
        {
            if (baseDatas.Count > 0)
                base.InitializeVisibleBaseDatas(baseDatas);
        }

        /// <inheritdoc/>
        protected override void InitializeVisibleInstanceDatas(List<ActorData> instanceDatas = null)
        {
            if (instanceDatas != null && instanceDatas.Count > 0)
                base.InitializeVisibleInstanceDatas(instanceDatas);
        }

        #region Loading

        /// <summary>
        ///
        /// </summary>
        /// <param name="actors"></param>
        /// <param name="instanceDatas"></param>
        protected virtual void LoadActors(List<ActorData> actors, List<ActorData> instanceDatas = null)
        {
            for (int i = 0; i < actors.Count; i++)
                AttemptLoadActor(i, actors, instanceDatas);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="actors"></param>
        /// <param name="instanceDatas"></param>
        protected virtual void AttemptLoadActor(int i, List<ActorData> actors, List<ActorData> instanceDatas = null)
        {
            for (int j = 0; j < ActorPrefabs.Count; j++)
                if (ActorPrefabs[j] != null && ActorPrefabs[j].name == actors[i].aReference)
                    if (instanceDatas != null && instanceDatas.Count > 0)
                        LoadActor(ActorPrefabs[j], (ActorData)actors[i].Clone(), VerifyData(instanceDatas, ActorPrefabs[j].name));
                    else
                        LoadActor(ActorPrefabs[j], (ActorData)actors[i].Clone());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceDatas"></param>
        /// <param name="nameLook"></param>
        /// <returns></returns>
        protected virtual ActorData VerifyData(List<ActorData> instanceDatas, string nameLook)
        {
            // This if statement has been added because when leaving damons dorm on FTE 1 
            // the game freezes going to the dorm hallway because it has dorm hallway instance data from the 
            // previous objective. why this happens hasn't been fixed but this code keeps this problem from making
            // an error freezing the game in a exported build.

            if (instanceDatas == null)
                UnityEngine.Debug.LogWarning("CRITICAL WARNING! INSTANCE LIST WAS NULL. Instance data might be leaking Through the game?");
            else if (instanceDatas.Count == 0)
                UnityEngine.Debug.Log("Actor Instance Data was empty.");

            if (instanceDatas == null || instanceDatas.Count == 0)
                return null;

            for (int i = 0; i < instanceDatas.Count; i++)
                if (instanceDatas[i].aReference == nameLook)
                    return instanceDatas[i];

            UnityEngine.Debug
                .LogWarning("The instance actor data list couldn't find the character it was looking for? " + nameLook);

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="shell"></param>
        /// <param name="actorData"></param>
        /// <param name="instanceData"></param>
        protected virtual void LoadActor(GameObject shell, ActorData actorData, ActorData instanceData = null)
        {
            GameObject spawnedActor = Instantiate(shell);

            Actor actor = spawnedActor.GetComponent<Actor>();

            ApplyActorData(actor, actorData);

            ApplyInstanceData(actor, actorData, instanceData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="actorData"></param>
        protected virtual void ApplyActorData(Actor actor, ActorData actorData)
        {
            actor.gameObject.name = actorData.aReference;

            ApplyBasicActorData(actor, actorData);

            actor.conversations.Clear();

            for (int i = 0; i < actorData.actorDialogues.Count; i++)
                actor.conversations.Add((LocalDialogue)actorData.actorDialogues[i].Clone());

            ApplyActorMaterial(actor, actorData.mat);

            ApplyActorSprite(actor, actorData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="actorData"></param>
        protected virtual void ApplyBasicActorData(Actor actor, ActorData actorData)
        {
            actor.transform.position = actorData.position;

            actor.transform.eulerAngles = actorData.rotation;

            actor.SetIdentified(actorData.identified);

            if (!string.IsNullOrEmpty(actorData.displayName))
                actor.DisplayName = actorData.displayName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="mat"></param>
        protected virtual void ApplyActorMaterial(Actor actor, Material mat)
        {
            var meshes = actor.GetComponentsInChildren<MeshRenderer>();

            meshes[0].material = mat;

            Material material = new(meshes[1].sharedMaterial.shader)
            {
                mainTexture = meshes[0].sharedMaterial.mainTexture
            };

            meshes[1].sharedMaterial = material;

            meshes[1].sharedMaterial.color = Color.black;

            meshes[1].sharedMaterial.renderQueue = 3000;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="actorData"></param>
        protected virtual void ApplyActorSprite(Actor actor, ActorData actorData)
        {
            var spriteRenderer = actor.GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer.sprite.name == actorData.matName)
                return;

            if (characterDatabase == null)
                UnityEngine.Debug.LogError("ALERT: Character Database Not Found?");

            Character character = characterDatabase.GetCharacter(actorData.aReference);

            if (character != null && character.Expressions != null &&
                !actorData.matName.Contains(character.Expressions[0].Sprite.name) &&
                !spriteRenderer.sprite.name.Contains("stare") &&
                !actorData.matName.Contains("Stare"))
            {
                for (int i = 0; i < character.Expressions.Count; i++)
                {
                    if (actorData.matName.Contains(character.Expressions[i].Sprite.name))
                    {
                        spriteRenderer.sprite = character.Sprites[i].Sprite;

                        break;
                    }
                }
            }
        }

        #endregion

        #region Apply Instance Data

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="actorData"></param>
        /// <param name="instanceData"></param>
        protected virtual void ApplyInstanceData(Actor actor, ActorData actorData, ActorData instanceData)
        {
            if (instanceData == null)
                return;

            ApplyBasicActorData(actor, instanceData);

            for (int i = 0; i < instanceData.actorDialogues.Count && i < actorData.actorDialogues.Count; i++)
                actor.conversations[i].SetTriggered(instanceData.actorDialogues[i].triggered);

            ApplyActorMaterialInstance(actor, actorData, instanceData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="actorData"></param>
        /// <param name="instanceData"></param>
        protected virtual void ApplyActorMaterialInstance(Actor actor, ActorData actorData, ActorData instanceData)
        {
            Character character = characterDatabase.GetCharacter(actorData.aReference);

            if (character == null)
                return;

            UnityEngine.Debug.Log("REPORT: MATERIAL NAME WAS: " + instanceData.matName);

            for (int i = 0; i < character.Expressions.Count; i++)
            {
                Expression expression = character.Expressions[i];

                if (instanceData.matName.Contains(expression.Sprite.name))
                {
                    ApplyActorMaterial(actor, expression.Sprite);

                    actor.GetComponentInChildren<SpriteRenderer>().sprite = character.Sprites[i].Sprite;

                    break;
                }
            }
        }

        #endregion
    }
}