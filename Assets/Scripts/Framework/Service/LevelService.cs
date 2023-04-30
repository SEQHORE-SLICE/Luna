using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Framework
{
    public class LevelService : IService
    {

        public GameObject Character;

        public async UniTask InitializeAsync()
        {
            Character = await ResourceService.LoadAssetAsync<GameObject>("Character");
        }
        public void Destroy()
        {
            Character = null;
        }
    }
}
