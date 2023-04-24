using Cysharp.Threading.Tasks;
using Utilities;
namespace Framework
{
    public class ResourceService : Singleton<ResourceService>, IService
    {

        public async UniTask InitializeAsync()
        {
            await UniTask.CompletedTask;
        }

        public void PostInitialize()
        {
        }
        public void Destroy()
        {
            throw new System.NotImplementedException();
        }

    }
}
