using System;
using Utilities;

namespace Framework
{
    public sealed class BehaviorProxy : MonoSingletonAuto<BehaviorProxy>
    {
        public Action OnUpdate;
        public Action OnLateUpdate;

        private void Update() => OnUpdate?.Invoke();
        private void LateUpdate() => OnLateUpdate?.Invoke();
    }
}
