using System;
using Cysharp.Threading.Tasks;
using Framework;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;
namespace Process
{
    public sealed partial class Character : IProcess
    {
        private Action _update;
        private Action _onDestroy;
        private GameObject _gameObject;

        private async UniTask CoreInit()
        {
            var prefab = await ResourceService.LoadAssetAsync<GameObject>("Character");
            _gameObject = Object.Instantiate(prefab);
            _gameObject.TryAddComponent(out _controller);
            _gameObject.transform.position = Vector3.zero;
        }

        public async void Initialization()
        {
            await CoreInit();
            await MovementInit();
            InputInit();
            BehaviorProxy.instance.OnUpdate += _update;
        }

        public void Destroy()
        {
            BehaviorProxy.instance.OnUpdate -= _update;
            _onDestroy?.Invoke();
        }
    }
}
