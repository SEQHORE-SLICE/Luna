using ConfigurationSO;
using Cysharp.Threading.Tasks;
using Framework;
using UnityEngine;
namespace Process
{
    public sealed partial class Character
    {
        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private const float TerminalVelocity = 53.0f;

        // timeout delta time
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private CharacterSO _characterSO;
        private CharacterController _controller;
        private Camera _mainCamera;
        public Vector3 position => _gameObject.transform.position;
        private Transform transform => _gameObject.transform;
        private async UniTask MovementInit()
        {
            _update += MovementUpdate;
            _mainCamera = Explorer.TryGetService<CameraService>().mainCamera;
            _characterSO = await ResourceService.LoadAssetAsync<CharacterSO>("Assets/Settings/CharacterSO.asset");
        }
        private void MovementUpdate()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
        }
        private void GroundedCheck()
        {
            // set sphere position, with offset
            var spherePosition = new Vector3(transform.position.x, transform.position.y - _characterSO.groundedOffset, transform.position.z);
            _characterSO.grounded = Physics.CheckSphere(spherePosition, _characterSO.groundedRadius, _characterSO.groundLayers, QueryTriggerInteraction.Ignore);
        }
        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _sprint ? _characterSO.sprintSpeed : _characterSO.moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _analogMovement ? _move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _characterSO.speedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else { _speed = targetSpeed; }
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * _characterSO.speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            var inputDirection = new Vector3(_move.x, 0.0f, _move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _characterSO.rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            var targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
        private void JumpAndGravity()
        {
            if (_characterSO.grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = _characterSO.fallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f) { _verticalVelocity = -2f; }

                // Jump
                if (_jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(_characterSO.jumpHeight * -2f * _characterSO.gravity);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f) { _jumpTimeoutDelta -= Time.deltaTime; }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = _characterSO.jumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f) { _fallTimeoutDelta -= Time.deltaTime; }

                // if we are not grounded, do not jump
                _jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < TerminalVelocity) { _verticalVelocity += _characterSO.gravity * Time.deltaTime; }
        }
    }
}
