//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Actors;
using DREditor.Characters;
using DREditor.EventObjects;
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation.Actors
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDRESaveable) + ", " + nameof(IDRELoadable) +
        "\n\n")]
    public class TrialActorManager : MonoBehaviour, IDRESaveable, IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveKey = "actorDatas";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<TActorData> debugViewingInstances = new();


        /// <summary>
        ///
        /// </summary>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            if (inTrialMode.Value && inDialogue.Value)
                saveData.Set(saveKey, SaveTrialActors());
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Load(Dictionary<string, object> saveDatas)
        {
            if (inTrialMode.Value)
                LoadTrialActors(saveDatas.Get<List<TActorData>>(saveKey));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual Actor[] InitializeTrialActors()
        {
            if (characterDatabase == null)
                characterDatabase = Resources.Load<CharacterDatabase>("Characters/CharacterDatabase");

            Actor[] actors = (Actor[])FindObjectsByType(typeof(Actor));

            if (actors.Length == 0)
            {
                UnityEngine.Debug.Log("There are no Actors in the scene!");

                return null;
            }

            return actors;
        }
        
        /// <summary>
        ///
        /// </summary>
        protected virtual List<TActorData> SaveTrialActors()
        {
            Actor[] actors = InitializeTrialActors();

            List<TActorData> actorInstances = new();

            for (int i = 0; i < actors.Length; i++)
            {
                GameObject actorGO = actors[i].gameObject;

                MeshRenderer mesh = actorGO.GetComponentInChildren<MeshRenderer>();

                Character character = characterDatabase.GetCharacter(actorGO.name);

                TActorData trialActorData = new(
                    actorGO.name,
                    actorGO,
                    actors[i].GetIdentified(),
                    character.GetSpriteLabelByTexName(mesh.material.mainTexture.name),
                    mesh.sharedMaterial
                );

                actorInstances.Add(trialActorData);
            }

            debugViewingInstances = actorInstances;

            return actorInstances;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadTrialActors(List<TActorData> trialActorDatas)
        {
            Actor[] actors = InitializeTrialActors();

            if (actors == null || trialActorDatas == null)
                return;

            for (int i = 0; i < actors.Length; i++)
                for (int j = 0; j < trialActorDatas.Count; j++)
                    if (actors[i].gameObject.name == trialActorDatas[j].aReference)
                        LoadTrialActor(actors[i], trialActorDatas[j]);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadTrialActor(Actor actor, TActorData data)
        {
            actor.transform.position = data.position;

            actor.transform.eulerAngles = data.rotation;

            MeshRenderer[] meshes = actor.GetComponentsInChildren<MeshRenderer>();

            Character character = characterDatabase.GetCharacter(data.aReference);

            Texture texture = character.GetSpriteByName(data.matName);

            if (meshes.Length > 0)
                meshes[0].material.mainTexture = texture;

            if (meshes.Length > 1)
                meshes[1].material.mainTexture = texture;
        }
    }
}