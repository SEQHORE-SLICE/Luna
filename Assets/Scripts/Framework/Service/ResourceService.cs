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

        public async UniTask<T> LoadAssesAsync<T>(string key)
        {
            var handle = Addressables.LoadAssetAsync<T>(key);
            var result = await handle;
            return result;
        }
    }
}
