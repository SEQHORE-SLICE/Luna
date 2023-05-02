using System;
using JetBrains.Annotations;
using UnityEngine;
namespace Utilities
{
    public static class GameObjectExtension
    {
        [NotNull]
        public static T GetComponentWithException<T>([CanBeNull] this GameObject gameObject) where T : Component
        {
            if (gameObject == null) throw new NullReferenceException("GameObject is null,please check.");
            var component = gameObject.GetComponent<T>();
            if (component == null) throw new NullReferenceException($"{nameof(T)} does not exist,please check.");
            return component;
        }

        public static void TryAddComponent<T>([NotNull] this GameObject gameObject, out T component) where T : Component
        {
            if (gameObject.TryGetComponent(out component))
            {
                return;
            }
            component = gameObject.AddComponent<T>();
        }
    }
}
