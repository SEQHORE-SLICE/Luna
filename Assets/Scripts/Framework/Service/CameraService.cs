using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;
namespace Framework
{
    public class CameraService : IService
    {
        #region variable
        public Camera mainCamera { get; private set; }
        #endregion

        public async UniTask InitializeAsync()
        {
            var mainCameraGameObject = GameObject.FindWithTag("MainCamera");
            mainCamera = mainCameraGameObject.GetComponentWithException<Camera>();
            await UniTask.CompletedTask;
        }

        public void Destroy()
        {
            mainCamera = null;
        }
    }
}
