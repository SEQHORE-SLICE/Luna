using System;
using JetBrains.Annotations;
using UnityEngine;
namespace Utilities
{
    public static class GameObjectExtension
    {
        [NotNull]
        public static T GetComponentWithException<T>([CanBeNull] this GameObject gameObject)
        {
            if (gameObject == null) throw new NullReferenceException("GameObject is null,please check.");
            var component = gameObject.GetComponent<T>();
            if (component == null) throw new NullReferenceException($"{nameof(T)} does not exist,please check.");
            return component;
        }
    }
}
