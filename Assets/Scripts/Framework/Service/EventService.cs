using Cysharp.Threading.Tasks;
namespace Framework
{
    public class EventService : IService
    {
        public async UniTask InitializeAsync()
        {
            await UniTask.CompletedTask;
        }
        public void Destroy() { }
    }
}
