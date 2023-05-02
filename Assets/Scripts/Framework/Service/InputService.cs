using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
namespace Framework
{
    public sealed class InputService : IService
    {
        private InputActionAsset _inputActionAsset;
        private PlayerInput _playerInput;
        public Action<Vector2> Move;

        public async UniTask InitializeAsync()
        {
            var handle = Addressables.LoadAssetAsync<InputActionAsset>("default");
            _inputActionAsset = await handle;
            if (_inputActionAsset == null) throw new NullReferenceException();


            var gameObject = new GameObject($"[{nameof(InputService)}]")
            {
                transform =
                {
                    parent = Boot.rootObject.transform
                }
            };

            _playerInput = gameObject.AddComponent<PlayerInput>();

            _inputActionAsset.Enable();
            _playerInput.actions = _inputActionAsset;
            _playerInput.defaultActionMap = "Player";
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.onActionTriggered += OnAction;
        }

        public void Destroy()
        {
            _playerInput.onActionTriggered -= OnAction;
            _inputActionAsset = null;
            _playerInput = null;
        }

        private void OnAction(InputAction.CallbackContext callback)
        {
            if (callback.action.name == "Move")
            {
                Move?.Invoke(callback.ReadValue<Vector2>());
            }
        }
    }
}
