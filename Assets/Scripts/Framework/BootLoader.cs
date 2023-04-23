﻿using System;
using System.Collections.Generic;
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
            await Boot.InitializeAsync(CollectionService());
        }
        
        private static List<IService> CollectionService() => new List<IService>
        {
            TransitionService.instance,
            ResourceService.instance,
            InputService.instance
        };


        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void LoaderCheck()
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            var scenePath = new string[sceneCount];
            for (var i = 0; i < sceneCount; i++)
            {
                scenePath[i] =
                    Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }

            var sceneName = scenePath.Select(path => path[(path.LastIndexOf('/') + 1)..]).ToArray();

            if (!sceneName.Contains("Persistent"))
            {
                throw new Exception("Persistent scene is lose!");

            }

        }
        #endif

    }
}
