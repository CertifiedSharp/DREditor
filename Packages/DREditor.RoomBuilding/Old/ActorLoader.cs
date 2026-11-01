//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Actors;
using DREditor.Characters;
using DREditor.LocalDialogues;
using DREditor.RoomBuilding.Implementation.Actors;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding.Old
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ActorLoader : MonoBehaviour
    {
        public static List<ActorData> Save()
        {
            CharacterDatabase characterDatabase = GetDatabase();
            

            List<GameObject> ActorPrefabs = new();
            if (ActorPrefabs.Count < characterDatabase.GetActorPrefabCount())
            {
                List<GameObject> p = new();

                foreach (Character c in characterDatabase.Characters)
                {
                    if (c.ActorPrefab != null)
                    {
                        UnityEngine.Debug.Log("Added " + c.ActorPrefab.name);

                        p.Add(c.ActorPrefab);
                    }
                }

                ActorPrefabs = p;
            }


            List<ActorData> ActorListIn = new();

            Actor[] x = (Actor[])FindObjectsByType(typeof(Actor));

            if (x.Length == 0)
            {
                UnityEngine.Debug.Log("There are no Actors in the scene!");

                return null;
            }

            for (int i = 0; i < x.Length; i++)
            {
                GameObject actor = x[i].gameObject.transform.parent.gameObject;

                for (int j = 0; j < ActorPrefabs.Count; j++) // Look through Actor references
                {
                    if (ActorPrefabs[j] != null && ActorPrefabs[j].name == actor.name) // Actor Reference found Save actor data
                    {
                        ActorData a = new(ActorPrefabs[j].name, x[i]);

                        ActorListIn.Add((ActorData)a.Clone());
                    }
                }
            }

            return ActorListIn;
        }

        public static void Load(RoomData room, RoomData instanceRoom = null)
        {
            CharacterDatabase characterDatabase = GetDatabase();

            List<GameObject> ActorPrefabs = SpawnActors(characterDatabase);

            // when something is built itll take the object and convert it there
            if (room != null && room.actorDatas != null && room.actorDatas.Count != 0)
            {
                List<ActorData> actors = room.actorDatas;
                for (int i = 0; i < actors.Count; i++)
                {
                    ActorData actor = (ActorData)actors[i].Clone();
                    //UnityEngine.Debug.Log("Actor prefab count: " + ActorPrefabs.Count);
                    for (int j = 0; j < ActorPrefabs.Count; j++) // Look through Actor references
                    {
                        // Actor prefab matches Base room name
                        if (ActorPrefabs[j] != null && ActorPrefabs[j].name == actor.aReference) 
                        {
                            var x = instanceRoom != null ? GetCorrectInst(instanceRoom.actorDatas, ActorPrefabs[j].name) : null;

                            LoadActor(ActorPrefabs[j], actor, x, characterDatabase);
                        }
                    }

                }
            }

            UnityEngine.Debug.Log("Finished Loading Actors");
        }

        static List<GameObject> SpawnActors(CharacterDatabase characterDatabase)
        {
            List<GameObject> ActorPrefabs = new();

            if (ActorPrefabs.Count < characterDatabase.GetActorPrefabCount())
            {
                List<GameObject> p = new();

                foreach (Character c in characterDatabase.Characters)
                {
                    if (c.ActorPrefab != null)
                    {
                        UnityEngine.Debug.Log("Added " + c.ActorPrefab.name);

                        p.Add(c.ActorPrefab);
                    }
                }

                ActorPrefabs = p;
            }

            return ActorPrefabs;
        }

        static CharacterDatabase GetDatabase()
        {
#if UNITY_EDITOR
            string[] _databaseguids = AssetDatabase.FindAssets("t:CharacterDatabase");
            if (_databaseguids.Length >= 1)
            {
                string _databasepath = AssetDatabase.GUIDToAssetPath(_databaseguids[0]);

                try
                {
                    EditorGUILayout.LabelField($"Loading CharacterDatabase at {_databasepath}.");
                }
                catch
                {

                }
                return AssetDatabase.LoadAssetAtPath<CharacterDatabase>(_databasepath);
            }
#endif
            return null;
        }

        static ActorData GetCorrectInst(List<ActorData> instList, string nameLook)
        {
            // This if statement has been added because when leaving damons dorm on FTE 1 
            // the game freezes going to the dorm hallway because it has dorm hallway instance data from the 
            // previous objective. why this happens hasn't been fixed but this code keeps this problem from making
            // an error freezing the game in a exported build.

            if (instList == null)
            {
                UnityEngine.Debug.LogWarning("CRITICAL WARNING! INSTANCE LIST WAS NULL." +
                    " Instance data might be leaking Through the game?");
                return null;
            }

            for (int i = 0; i < instList.Count; i++)
            {
                if (instList[i].aReference == nameLook)
                    return instList[i];
            }

            UnityEngine.Debug.LogWarning("The instance actor data list couldn't find the character it was looking for? " + nameLook);
            return null;
        }

        public static void LoadActor(GameObject shell, ActorData actorData, ActorData instanceData = null,
            CharacterDatabase characterDatabase = null)
        {
            Actor a = SpawnActor(shell, actorData);

            if (characterDatabase == null)
                characterDatabase = GetDatabase();

            // This is so the sprite is loaded correctly, but I need to add a way to reference
            // the character database, I'm too lazy rn lmao
            if (a.GetComponentInChildren<SpriteRenderer>().sprite.name != actorData.matName)
            {
                Character character = null;

                if (characterDatabase != null)
                {
                    character = characterDatabase.GetCharacter(actorData.aReference);
                }
                else
                {
                    UnityEngine.Debug.LogError("Character Database Not Found?");
                }

                if (character != null && character.Expressions != null &&
                    !actorData.matName.Contains(character.Expressions[0].Sprite.name) &&
                    !a.GetComponentInChildren<SpriteRenderer>().sprite.name.Contains("stare") &&
                    !actorData.matName.Contains("Stare"))
                {

                    //UnityEngine.Debug.Log("Materials and sprites were not loaded the same for: " + actorData.matName + " , "
                    //+ a.GetComponentInChildren<SpriteRenderer>().sprite.name);

                    //UnityEngine.Debug.Log("MAT NAME WAS: " + actorData.matName);
                    for (int i = 0; i < character.Expressions.Count; i++)
                    {
                        Expression exp = character.Expressions[i];

                        if (actorData.matName.Contains(exp.Sprite.name))
                        {
                            a.GetComponentInChildren<SpriteRenderer>().sprite = character.Sprites[i].Sprite;
                            break;
                        }
                    }
                }
            }


            if (Application.isPlaying)
            {
                // The spawned object registers itself.
                //ServiceLocator.GetService<ActorManager>().RegisterObject(a);
            }


            LoadActorInstance(a, actorData, instanceData, characterDatabase);
        }


        protected static void LoadActorInstance(Actor a, ActorData actorData, ActorData instanceData = null,
            CharacterDatabase characterDatabase = null)
        {
            if (instanceData != null)
            {
                Character character = characterDatabase.GetCharacter(actorData.aReference);
                a.gameObject.transform.position = instanceData.position;
                a.gameObject.transform.eulerAngles = instanceData.rotation;
                a.SetIdentified(instanceData.identified);
                if (instanceData.displayName != "")
                {
                    a.DisplayName = instanceData.displayName;
                }
                for (int i = 0; i < instanceData.actorDialogues.Count && i < actorData.actorDialogues.Count; i++)
                {
                    UnityEngine.Debug.Log(i);
                    a.conversations[i].SetTriggered(instanceData.actorDialogues[i].triggered);
                }

                UnityEngine.Debug.Log("MAT NAME WAS: " + instanceData.matName);
                for (int i = 0; i < character.Expressions.Count; i++)
                {
                    Expression exp = character.Expressions[i];

                    if (instanceData.matName.Contains(exp.Sprite.name))
                    {
                        UnityEngine.Debug.Log(exp.Sprite.name);
                        var instanceMeshes = a.GetComponentsInChildren<MeshRenderer>();

                        instanceMeshes[0].material = exp.Sprite;

                        Material ma = new(instanceMeshes[1].sharedMaterial.shader);
                        ma.mainTexture = instanceMeshes[0].sharedMaterial.mainTexture;
                        instanceMeshes[1].sharedMaterial = ma;
                        instanceMeshes[1].sharedMaterial.color = Color.black;
                        instanceMeshes[1].sharedMaterial.renderQueue = 3000;
                        a.GetComponentInChildren<SpriteRenderer>().sprite = character.Sprites[i].Sprite;
                        break;
                    }
                }
            }
        }

        protected static Actor SpawnActor(GameObject shell, ActorData actorData)
        {
            GameObject loaded = Instantiate(shell);
            loaded.name = actorData.aReference;
            loaded.transform.position = actorData.position;
            loaded.transform.eulerAngles = actorData.rotation;
            Actor a = loaded.GetComponent<Actor>();

            a.conversations.Clear();

            for (int i = 0; i < actorData.actorDialogues.Count; i++)
                a.conversations.Add((LocalDialogue)actorData.actorDialogues[i].Clone());

            var meshes = a.GetComponentsInChildren<MeshRenderer>();

            meshes[0].material = actorData.mat;
            Material m = new(meshes[1].sharedMaterial.shader);
            m.mainTexture = meshes[0].sharedMaterial.mainTexture;
            meshes[1].sharedMaterial = m;
            meshes[1].sharedMaterial.color = Color.black;
            meshes[1].sharedMaterial.renderQueue = 3000;
            a.SetIdentified(actorData.identified);
            if (actorData.displayName != "")
                a.DisplayName = actorData.displayName;
            return a;
        }

        public static void UnloadActors(bool inEditor)
        {
            CharacterDatabase characterDatabase = GetDatabase();

            List<GameObject> ActorPrefabs = new();
            if (ActorPrefabs.Count < characterDatabase.GetActorPrefabCount())
            {
                List<GameObject> p = new();

                foreach (Character c in characterDatabase.Characters)
                {
                    if (c.ActorPrefab != null)
                    {
                        UnityEngine.Debug.Log("Added " + c.ActorPrefab.name);

                        p.Add(c.ActorPrefab);
                    }
                }

                ActorPrefabs = p;
            }

            // Add pop up window that says "Are you sure you want to unload actors? this will remove ALL actors in the scene."
            // Destroy current Actor Objects in the scene

            Actor[] x = (Actor[])FindObjectsByType(typeof(Actor));

            if (x.Length == 0)
            {
                UnityEngine.Debug.Log("There are no Referenced Actors to Unload in the scene!");

                return;
            }

            for (int i = 0; i < x.Length; i++)
            {
                GameObject actor = x[i].gameObject.transform.parent.gameObject;
                for (int j = 0; j < ActorPrefabs.Count; j++) // Look through Actor references
                {
                    if (ActorPrefabs[j] != null && ActorPrefabs[j].name == actor.name) // Found
                    {
                        if (inEditor)
                            DestroyImmediate(actor);
                        else
                            Destroy(actor);

                        break;
                    }
                }

            }
        }


        

    #if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new();

            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null)
                    assets.Add(asset);
            }

            return assets;
        }
    #endif
    }

}