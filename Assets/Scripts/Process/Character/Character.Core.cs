using System;
using Framework;
using UnityEngine;
namespace Process
{
    public sealed partial class Character : IProcess
    {
        private Action _update;
        private readonly GameObject _gameObject;

        public Character(Vector3 pos)
        {

            _gameObject = new GameObject("Character")
            {
                transform =
                {
                    position = pos
                }
            };



        }

        public void Initialization()
        {
            MovementInit();
        }

        public void Destroy()
        {
            BehaviorProxy.instance.OnUpdate -= _update;
        }
    }
}
