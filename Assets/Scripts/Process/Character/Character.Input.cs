using Framework;
using UnityEngine;
namespace Process
{
    public sealed partial class Character
    {

        private Vector2 _move;
        private bool _analogMovement;
        private bool _sprint;
        private bool _jump;

        private void InputInit()
        {
            var inputService = Explorer.TryGetService<InputService>();
            inputService.Move += OnMove;
            _onDestroy += () => { inputService.Move += OnMove; };
        }
        private void OnMove(Vector2 vector2) => _move = vector2;
    }
}
