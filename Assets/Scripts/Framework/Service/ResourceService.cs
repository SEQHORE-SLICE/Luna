using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;
namespace Framework
{
    public class ResourceService : Singleton<ResourceService>, IService
    {

        public async UniTask InitializeAsync()
        {
            Boot.AddPostInitializationTask(Test);
            
            await UniTask.CompletedTask;
        }

        public void PostInitialize()
        { }
        public void Destroy()
        {
            throw new System.NotImplementedException();
        }

        private static async UniTask Test()
        {
          
            if (InputService.instance._inputActionAsset == null)
            {
                Debug.Log("?");
            }
            else
            {
                var path = InputService.instance._inputActionAsset.name;
                Debug.Log(path);
            }
            await UniTask.CompletedTask;
        }
    }
}
