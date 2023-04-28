using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;
using Task = System.Threading.Tasks.Task;
namespace Framework
{
    public class ResourceService : Singleton<ResourceService>, IService
    {

        public async UniTask InitializeAsync()
        {
            var task = Task.Run(Test);
            Boot.AddPostInitializationTask(task);

            await UniTask.CompletedTask;
        }

        public void Destroy() { }

        private static void Test()
        {

            if (InputService.instance.InputActionAsset == null)
            {
                Debug.Log("?");
            }
            else
            {
                string path = InputService.instance.InputActionAsset.name;
                Debug.Log(path);
            }
        }
    }
}
