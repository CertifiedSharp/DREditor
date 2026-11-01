//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.RoomStateBuilding;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public abstract class RoomStateHandlerBase<T, T2> : MonoBehaviour, IRoomStateSaver, IRoomStateLoader where T : StateDataAsset<T2>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] string handlerTypeName = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] string dataAssetName = "";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<T2> loadedBaseDatas;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<T2> loadedInstances = null;


        public virtual string GetTypeName() => handlerTypeName;

        public virtual void LoadData(List<ScriptableObject> roomSO)
        {
            T data = GetDataAsset<T>(roomSO);

            if (data != null)
                LoadAssetData(data.datas);
        }

        public virtual void LoadInstance(List<ScriptableObject> roomSO, object instanceData)
        {
            string instanceJson = (string)instanceData;

            List<T2> instanceAssetData = new();

            try
            {
                var assetData = JsonUtility.FromJson<InstanceListWrapper<InstanceListWrapper<string>>>(instanceJson);

                foreach (var stateSection in assetData.wrappedInstances)
                {
                    foreach (var dataInstance in stateSection.wrappedInstances)
                    {
                        try
                        {
                            if (LoadedInstanceValid(dataInstance, out T2 result))
                                instanceAssetData.Add(result);
                        }
                        catch { }

                        //break; // Commented out because actor instance data wasn't being loaded properly.
                        // But it is unknown if this break was actually needed.
                    }
                }
            }
            catch
            {
                instanceAssetData = JsonUtility.FromJson<List<T2>>(instanceJson);
            }

            T data = GetDataAsset<T>(roomSO);

            if (data != null)
                LoadAssetData(data.datas, instanceAssetData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataInstance"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool LoadedInstanceValid(string dataInstance, out T2 result)
        {
            string emptyBaseline = JsonUtility.ToJson(JsonUtility.FromJson<T2>("{}"));

            result = JsonUtility.FromJson<T2>(dataInstance);

            return JsonUtility.ToJson(result) != emptyBaseline;
        }

        public virtual ScriptableObject SaveDataAsset(string path)
        {
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}",
                new[] { Path.GetDirectoryName(path.TrimEnd(Path.DirectorySeparatorChar)) });

            bool overridingExistingData = guids.Length > 0;

            T dataAsset;

            if (overridingExistingData)
                dataAsset = GetAssetFromGUID<T>(guids[0]);
            else
                dataAsset = ScriptableObject.CreateInstance<T>();

            dataAsset.SetDatas(Save());

            if (!overridingExistingData && dataAsset.datas != null && dataAsset.datas.Count != 0)
                AssetDatabase.CreateAsset(dataAsset, path + "_" + GetDataAssetName() + ".asset");

            EditorUtility.SetDirty(dataAsset);

            AssetDatabase.SaveAssets();

            return overridingExistingData ? null : dataAsset;
#else
            return null;
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDataAssetName() => dataAssetName;


        public virtual object SaveInstance()
        {
            List<T2> dataInstances = Save();

            List<string> strings = new();

            if (dataInstances == null)
                return strings;

            foreach (T2 dataInstance in dataInstances)
            {
                if (dataInstance != null)
                {
                    string serializedJson = JsonUtility.ToJson(dataInstance);

                    strings.Add(serializedJson);
                }
            }

            return strings;
        }

        public abstract void Unload(bool inEditor);

#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T3"></typeparam>
        /// <param name="guid"></param>
        /// <returns></returns>
        protected T3 GetAssetFromGUID<T3>(string guid) where T3 : UnityEngine.Object
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            return AssetDatabase.LoadAssetAtPath<T3>(path);
        }
#endif
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T4"></typeparam>
        /// <param name="scriptableObjects"></param>
        /// <returns></returns>
        protected T4 GetDataAsset<T4>(List<ScriptableObject> scriptableObjects)
        {
            foreach (var scriptableObject in scriptableObjects)
                if (scriptableObject is T4 data)
                    return data;

            return default;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract List<T2> Save();

        /// <summary>
        ///
        /// </summary>
        /// <param name="baseDatas"></param>
        /// <param name="instanceDatas"></param>
        protected virtual void LoadAssetData(List<T2> baseDatas, List<T2> instanceDatas = null)
        {
            InitializeVisibleBaseDatas(baseDatas);

            InitializeVisibleInstanceDatas(instanceDatas);

            LoadAssetProcess(baseDatas, instanceDatas);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="baseDatas"></param>
        protected virtual void InitializeVisibleBaseDatas(List<T2> baseDatas) => loadedBaseDatas = baseDatas;

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceDatas"></param>
        protected virtual void InitializeVisibleInstanceDatas(List<T2> instanceDatas = null) => loadedInstances = instanceDatas;

        /// <summary>
        ///
        /// </summary>
        /// <param name="baseDatas"></param>
        /// <param name="instanceDatas"></param>
        protected abstract void LoadAssetProcess(List<T2> baseDatas, List<T2> instanceDatas = null);
    }
}