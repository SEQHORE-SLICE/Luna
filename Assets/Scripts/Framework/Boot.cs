using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
namespace Framework
{
    public static class Boot
    {
        private const string SystemInstanceName = "SYSTEM";
        private const string PersistenceSceneName = "Persistent";
        public static GameObject rootObject { get; private set; }
        private static UniTaskCompletionSource _initializeTcs;
        private static bool asyncInitialized => _initializeTcs != null && _initializeTcs.Task.GetAwaiter().IsCompleted;
        private static bool asyncInitializing => _initializeTcs != null && !_initializeTcs.Task.GetAwaiter().IsCompleted;
        internal static readonly List<IService> SystemServices = new List<IService>();
        private static readonly List<UniTask> PostInitializationTasks = new List<UniTask>();
        public static void AddPostInitializationTask(UniTask task) => PostInitializationTasks.Add(task);
        public static void AddPostInitializationTask(IEnumerable<UniTask> tasks) => PostInitializationTasks.AddRange(tasks);
        [RuntimeInitializeOnLoadMethod] public static async void BootLoader()
        {
            rootObject = new GameObject(SystemInstanceName, typeof(BehaviorProxy))
            {
                tag = "GameController"
            };
            await InitializeAsync();
            AddExit();
        }
        private static void AddExit()
        {
            var button = GameObject.Find("ExitButton");
            var buttonCom = button.GetComponent<Button>();
            buttonCom.onClick.AddListener(Exit);
        }
        /// <summary>
        ///     Allocate all resources
        /// </summary>
        private static async UniTask InitializeAsync()
        {
            //start when the current task is not running
            //block when the current task is running
            //prevent multiple initialize
            #region ====Set Semaphore====
            if (asyncInitialized) return;
            if (asyncInitializing)
            {
                await _initializeTcs.Task;
                return;
            }
            #endregion

            //Open block-semaphore
            _initializeTcs = new UniTaskCompletionSource();
            
            #region ====Initialize Scene====
            var scene = SceneManager.GetSceneByName(PersistenceSceneName);
            if (!scene.IsValid())
            {
                await SceneManager.LoadSceneAsync(PersistenceSceneName, LoadSceneMode.Additive);
                scene = SceneManager.GetSceneByName(PersistenceSceneName);
            }
            SceneManager.MoveGameObjectToScene(rootObject, scene);
            #endregion
            #region ====Initialize Service====
            //Collect service initialization information
            var serviceAssembly = typeof(IService).Assembly;
            var assemblyTypes = serviceAssembly.GetTypes();
            var serviceTypes = assemblyTypes.Where(type => type.GetInterfaces().Contains(typeof(IService))).ToArray();
            foreach (var service in serviceTypes)
            {
                if (serviceAssembly.CreateInstance(service.ToString()) is not IService instance) throw new WarningException($"There has some error in {service} class");
                #if UNITY_EDITOR
                //Easy to manage
                new GameObject($"[{service.Name}]").transform.SetParent(rootObject.transform);
                #endif
                SystemServices.Add(instance);
            }

            //Wait for service initialization to complete
            foreach (var service in SystemServices)
            {
                await service.InitializeAsync();
                if (!asyncInitializing) return;
            }
            await UniTask.WhenAll(PostInitializationTasks);
            #endregion

            //Close block-semaphore
            _initializeTcs.TrySetResult();
        }
        /// <summary>
        ///     Release all resources
        /// </summary>
        private static void Destruction()
        {
            _initializeTcs = null;
            SystemServices.ForEach(service => service.Destroy());
            SystemServices.Clear();
            Object.Destroy(rootObject);
        }
        private static void Exit()
        {
            #if UNITY_EDITOR
            Destruction();
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
         #if UNITY_EDITOR
        [InitializeOnLoadMethod] public static void LoaderCheck()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            string[] scenePath = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++) { scenePath[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)); }
            string[] sceneName = scenePath.Select(path => path[(path.LastIndexOf('/') + 1)..]).ToArray();
            if (!sceneName.Contains("Persistent")) { throw new Exception("Persistent scene is lose!"); }
        }
        #endif
    }
}
