using UnityEngine;
using Utilities;
namespace Process
{
    public sealed partial class Character
    {
        public Vector3 position => _gameObject.transform.position;
        private Rigidbody _rigidbody;



        private void MovementInit()
        {
            _rigidbody = _gameObject.GetComponentWithException<Rigidbody>();
            _update += MovementUpdate;
        }


        private void MovementUpdate()
        {

        }
    }
}
