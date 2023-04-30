using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Framework
{
    public class BootLoader : MonoBehaviour
    {

        public Button exit;
        public async void Awake()
        {
            await Boot.InitializeAsync(gameObject);
            exit.onClick.AddListener(Exit);
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

        private void Exit()
        {
            #if UNITY_EDITOR
            Boot.Destruction();
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
