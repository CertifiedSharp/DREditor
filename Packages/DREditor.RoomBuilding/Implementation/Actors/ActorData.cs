//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Actors;
using DREditor.LocalDialogues;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.RoomBuilding.Implementation.Actors
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ActorData
    {
        /// <summary>
        ///
        /// </summary>
        public string aReference;

        /// <summary>
        ///
        /// </summary>
        public Vector3 position;

        /// <summary>
        ///
        /// </summary>
        public Vector3 rotation;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("aData")]
        public List<LocalDialogue> actorDialogues;

        /// <summary>
        ///
        /// </summary>
        public bool identified = true;

        /// <summary>
        ///
        /// </summary>
        public Material mat;

        /// <summary>
        ///
        /// </summary>
        public string matName = "";

        /// <summary>
        ///
        /// </summary>
        public string displayName = "";


        public ActorData() { }

        public ActorData(string refName, Actor actor)
        {
            position = actor.transform.position;

            rotation = actor.transform.eulerAngles;

            aReference = refName;

            actorDialogues = actor.conversations;

            var mesh = actor.GetComponentInChildren<MeshRenderer>();

            mat = mesh.sharedMaterial;

            matName = mesh.sharedMaterial.name;

            identified = actor.GetIdentified();

            displayName = actor.GetSaveDisplayName();
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            ActorData newActorData = new();

            ActorData copy = (ActorData)MemberwiseClone();

            newActorData.aReference = copy.aReference;

            newActorData.position = copy.position;

            newActorData.rotation = copy.rotation;

            newActorData.mat = copy.mat;

            newActorData.matName = copy.matName;

            newActorData.identified = copy.identified;

            newActorData.displayName = copy.displayName;

            newActorData.actorDialogues = new List<LocalDialogue>();

            if (copy.actorDialogues != null)
                foreach (LocalDialogue localDialogue in copy.actorDialogues)
                    newActorData.actorDialogues.Add((LocalDialogue)localDialogue.Clone());

            return newActorData;
        }
    }
}