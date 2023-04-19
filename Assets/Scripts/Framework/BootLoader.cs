using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Framework
{
    public class BootLoader : MonoBehaviour
    {
        public void Awake()
        {
            Boot.InitializeAsync().Forget();
        }
        public void Start()
        {

        }
    }
}
