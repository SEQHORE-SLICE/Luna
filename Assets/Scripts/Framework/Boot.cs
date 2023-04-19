using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Framework
{
    public static class Boot
    {
        /// <summary>
        ///     The name of the system instance
        /// </summary>
        public const string SystemInstanceName = "SYSTEM";
        public const string PersistenceSceneName = "Persistent";
        public static GameObject systemInstanceObject { get; private set; }

        private static UniTaskCompletionSource _initializeTcs;

        public static bool asyncInitialized => _initializeTcs != null && _initializeTcs.Task.GetAwaiter().IsCompleted;
        public static bool asyncInitializing => _initializeTcs != null && !_initializeTcs.Task.GetAwaiter().IsCompleted;

        private static readonly List<IService> SystemServices = new List<IService>();

        /// <summary>
        ///     Allocate all resources
        /// </summary>
        internal static async UniTask InitializeAsync()
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
            systemInstanceObject = new GameObject(SystemInstanceName);
            var scene = SceneManager.GetSceneByName(PersistenceSceneName);
            if (!scene.IsValid())
            {
                await SceneManager.LoadSceneAsync(PersistenceSceneName, LoadSceneMode.Additive);
                scene = SceneManager.GetSceneByName(PersistenceSceneName);
            }

            SceneManager.MoveGameObjectToScene(systemInstanceObject, scene);
            #endregion

            #region ====Initialize Service====
            //Collect service initialization information

            var serviceAssembly = typeof(IService).Assembly;
            var assemblyTypes = serviceAssembly.GetTypes();
            var serviceTypes = assemblyTypes.Where(type => type.GetInterfaces().Contains(typeof(IService))).ToArray();

            foreach (var service in serviceTypes)
            {
                if (serviceAssembly.CreateInstance(service.ToString()) is not IService instance)
                    throw new WarningException($"There has some error in {service} class");
                #if UNITY_EDITOR
                //Easy to manage
                new GameObject($"[{service.Name}]").transform.SetParent(systemInstanceObject.transform);
                #endif
                SystemServices.Add(instance);
            }

            //Wait for service initialization to complete

            foreach (var service in SystemServices)
            {
                service.InitializeAsync();
                if (!asyncInitializing) return;
            }
            #endregion

            //Close block-semaphore
            _initializeTcs.TrySetResult();
        }

        /// <summary>
        ///     Release all resources
        /// </summary>
        internal static void Destruction()
        {
            _initializeTcs = null;
            SystemServices.ForEach(service => service.Destroy());
            SystemServices.Clear();
            Object.Destroy(systemInstanceObject);
        }
    }
}
