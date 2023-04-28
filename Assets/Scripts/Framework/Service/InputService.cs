using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using Utilities;
namespace Framework
{
    public class InputService : Singleton<InputService>, IService
    {

        internal InputActionAsset InputActionAsset;

        public async UniTask InitializeAsync()
        {
            var handle = Addressables.LoadAssetAsync<InputActionAsset>("default");
            InputActionAsset = await handle;
        }

        public void Destroy() { }

    }
}
