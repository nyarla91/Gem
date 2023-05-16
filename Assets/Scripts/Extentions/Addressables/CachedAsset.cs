using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Extentions.Addressables
{
    [Serializable]
    public class CachedAsset<T>
    {
        [field: SerializeField] public AssetReference Reference { get; private set; }
        public T Asset { get; private set; }
        
        public async Task Load()
        {
            Asset = await Reference.LoadAssetAsync<T>().Task;
        }

        public void Unload(T defaultValue)
        {
            Asset = defaultValue;
            Reference.ReleaseAsset();
        }
    }
}