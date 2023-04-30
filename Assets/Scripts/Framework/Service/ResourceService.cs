using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
namespace Framework
{
    public class ResourceService : IService
    {
        public async UniTask InitializeAsync()
        {
            await UniTask.CompletedTask;
        }

        public void Destroy() { }

        public static async UniTask<T> LoadAssetAsync<T>(string key)
        {
            var handle = Addressables.LoadAssetAsync<T>(key);
            var result = await handle;
            return result;
        }
    }
}
