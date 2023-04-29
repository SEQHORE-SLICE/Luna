using UnityEngine;
namespace Utilities
{
    /// <summary>
    /// Automatically instantiated in Awake and destroyed in OnDestroy.
    /// </summary>
    /// <remarks>
    /// MonoBehaviour needs to be created manually.
    /// </remarks>
    /// <typeparam name="T">The class itself</typeparam>
    public abstract class MonoSingletonAuto<T> : MonoBehaviour where T : MonoSingletonAuto<T>
    {
        public static T instance { get; private set; }

        protected virtual void OnEnable()
        {
            if (instance != this) instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this) instance = null;
        }
    }
}
