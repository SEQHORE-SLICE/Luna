using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using Utilities;
namespace Framework
{
    public class InputService : Singleton<InputService>, IService
    {

        internal InputActionAsset _inputActionAsset;
       
        public async UniTask InitializeAsync()
        {
            var handle = Addressables.LoadAssetAsync<InputActionAsset>("default").ToUniTask();
            instance._inputActionAsset = await handle;
        }

        public void PostInitialize() { }

        public void Destroy() { }


    }
}
