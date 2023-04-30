using System.Collections.Generic;
using Process;
using UnityEngine;
namespace SandBox
{
    public class CharacterInit : MonoBehaviour
    {
        private readonly List<IProcess> _processes = new List<IProcess>();
       
        public void Start()
        {
            var a = new Character();
            _processes.Add(a);

            foreach (var process in _processes)
            {
                process.Initialization();
            }
          
        }
    }
}
