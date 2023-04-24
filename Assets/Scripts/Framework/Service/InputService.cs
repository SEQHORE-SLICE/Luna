using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using Utilities;
namespace Framework
{
    public class InputService : Singleton<InputService>, IService
    {

        private InputActionAsset _inputActionAsset;

        public async UniTask InitializeAsync()
        {
            var task = Addressables.LoadAssetAsync<InputActionAsset>("default").ToUniTask();
            _inputActionAsset = await task;
        }

        public void PostInitialize() { }

        public void Destroy() { }


        [CanBeNull]
        public InputAction GetInputAction(string mapName, string actionName)
        {
            var map = _inputActionAsset.FindActionMap(mapName);
            return map?.FindAction(actionName);
        }

    }
}
