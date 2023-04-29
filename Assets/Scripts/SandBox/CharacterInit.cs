using System;
using System.Collections.Generic;
using Framework;
using Process;
using UnityEngine;
namespace SandBox
{
    public class CharacterInit : MonoBehaviour
    {
        private readonly List<IProcess> _processes = new List<IProcess>();
        public Vector2 value;
        public void Start()
        {
            _processes.Add(new Character(Vector3.one));

            foreach (var process in _processes)
            {
                process.Initialization();
            }
            var inputService = Explorer.TryGetService<InputService>();

            inputService.Move += vector2 => {value = vector2;};
        }
    }
}
