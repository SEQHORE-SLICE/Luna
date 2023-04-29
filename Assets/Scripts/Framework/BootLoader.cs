using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Framework
{
    public class BootLoader : MonoBehaviour
    {

        public async void Awake()
        {
            await Boot.InitializeAsync(gameObject);
        }

        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void LoaderCheck()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            string[] scenePath = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenePath[i] =
                    Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }

            string[] sceneName = scenePath.Select(path => path[(path.LastIndexOf('/') + 1)..]).ToArray();

            if (!sceneName.Contains("Persistent"))
            {
                throw new Exception("Persistent scene is lose!");
            }
        }
        #endif

    }
}
