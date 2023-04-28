using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utilities;
namespace Framework
{
    public class TransitionService : Singleton<TransitionService>, IService
    {
        public async UniTask InitializeAsync() => await UniTask.CompletedTask;

        public void Destroy() { }

        /// <summary>
        ///     Unload tag scene and load next scene asynchronously
        /// </summary>
        /// <remarks>Use Forget method to return void</remarks>
        /// <param name="loadName">scene name to load</param>
        /// <param name="unloadName">scene name to unload,default is the active scene</param>
        /// <param name="postAction">method to do after switch</param>
        public async UniTask TransitionScene(string loadName, string unloadName = null, UnityAction postAction = null)
        {
            //unload before load to avoid some bugs 
            if (!SceneManager.GetSceneByName(unloadName).IsValid())
                throw new Exception($"Please check {unloadName} scene,it doesn't exist!");

            //Check before loading
            if (!SceneManager.GetSceneByName(loadName).IsValid())
                throw new Exception($"Please check {loadName} scene,it doesn't exist!");

            //asynchronous loading and unloading
            await
            (
                SceneManager.UnloadSceneAsync(unloadName).ToUniTask(),
                SceneManager.LoadSceneAsync(loadName, LoadSceneMode.Additive).ToUniTask()
            );

            //TODO:need to verify correctness
            //select the newest scene and set it active
            var targetScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(targetScene);
            postAction?.Invoke();
        }
    }
}
