using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SceneSelectAttribute : PropertyAttribute
    {
        public string[] nameList => AllSceneNames();

        private static string[] AllSceneNames()
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            var scenePath = new string[sceneCount];
            for (var i = 0; i < sceneCount; i++)
            {
                scenePath[i] =
                    Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }

            return scenePath.Select(path => path[(path.LastIndexOf('/') + 1)..]).ToArray();
        }
    }
}
