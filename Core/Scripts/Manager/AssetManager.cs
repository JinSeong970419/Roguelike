using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
    public class AssetManager : MonoSingleton<AssetManager>
    {
        private Dictionary<string, GameObject> assets = new Dictionary<string, GameObject>();
        private HashSet<string> readyAssets = new HashSet<string>();
        private HashSet<string> unloadAssets = new HashSet<string>();


        protected override void Awake()
        {
            base.Awake();
            Addressables.InitializeAsync();
        }

        public static void LoadAsset(AssetReference assetReference)
        {
            if (assetReference == null) return;
            if (assetReference.RuntimeKeyIsValid() == false) return;
            if (Instance.readyAssets.Contains(assetReference.AssetGUID)) return;
            if (Instance.assets.ContainsKey(assetReference.AssetGUID)) return;

            Instance.readyAssets.Add(assetReference.AssetGUID);
            
            assetReference.InstantiateAsync().Completed += (handle) =>
            {
                if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    handle.Result.SetActive(false);
                    Instance.readyAssets.Remove(assetReference.AssetGUID);
                    if (Instance.unloadAssets.Contains(assetReference.AssetGUID))
                    {
                        assetReference.ReleaseInstance(handle.Result);
                        Instance.unloadAssets.Remove(assetReference.AssetGUID);
                        return;
                    }

                    Instance.assets.Add(assetReference.AssetGUID, handle.Result);
                }
            };
        }

        public static void UnloadAsset(AssetReference assetReference)
        {
            if (assetReference == null) return;
            if (assetReference.RuntimeKeyIsValid() == false) return;

            if (Instance.assets.TryGetValue(assetReference.AssetGUID, out GameObject obj))
            {
                assetReference.ReleaseInstance(obj);
            }
            else
            {
                if (Instance.unloadAssets.Contains(assetReference.AssetGUID) == false)
                {
                    Instance.unloadAssets.Add(assetReference.AssetGUID);
                }
            }
        }

        public static GameObject Instantiate(AssetReference assetReference, Transform parent = null, bool instantiateInWorldSpace = false)
        {
            if (assetReference == null)
            {
                Debug.LogError("AssetReference is null.");
                return null;
            }
            if (assetReference.RuntimeKeyIsValid() == false)
            {
                Debug.LogError("AssetReference is invalid.");
                return null;
            }

            if (Instance.assets.TryGetValue(assetReference.AssetGUID, out GameObject obj))
            {
                var instanceObj = Instantiate(obj, parent, instantiateInWorldSpace);
                instanceObj.SetActive(true);
                return instanceObj;
            }
            else
            {
                LoadAsset(assetReference);
                return null;
            }
        }
    }
}
