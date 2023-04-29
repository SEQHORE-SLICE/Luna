using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;
namespace Framework
{
    public class CameraService :  IService
    {

        #region variable
        public Camera mainCamera { get; private set; }
        public Camera uiCamera { get; private set; }
        #endregion

        public async UniTask InitializeAsync()
        {
            var mainCameraGameObject = GameObject.FindWithTag("MainCamera");
            mainCamera = mainCameraGameObject.GetComponentWithException<Camera>();

            var uiCameraGameObject = GameObject.FindWithTag("UICamera");
            uiCamera = uiCameraGameObject.GetComponentWithException<Camera>();
            await UniTask.CompletedTask;
        }

        public void Destroy()
        {
            throw new System.NotImplementedException();
        }
    }
}