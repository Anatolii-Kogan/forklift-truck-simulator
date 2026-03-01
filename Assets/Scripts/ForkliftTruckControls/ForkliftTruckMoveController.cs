using Forklift.Components;
using UnityEngine;

namespace Forklift.Player
{
    public class ForkliftTruckMoveController : MonoBehaviour
    {
        [Space]
        [SerializeField] private float _engineForce = 1200f;
        [SerializeField] private float _maxSpeed = 5f;

        [Space]
        [SerializeField] private float _steerFactor = 1f;
        [SerializeField] private float _maxYawRate = 2.5f;

        [Space]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _anchor;

        private IRearWheelsController _rearWheels;
        private float _wheelBase;

        private float _direction;

        public void Init(IRearWheelsController rearWheels, Vector3 rearWheelsPosition)
        {
            _rearWheels = rearWheels;
            _wheelBase = (_anchor.position - rearWheelsPosition).magnitude;
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Move(float direction)
        {
            _direction = direction;
        }

        private void Move()
        {
            var up = _anchor.up;
            var forward = Vector3.ProjectOnPlane(_anchor.forward, up).normalized;

            var speed = Vector3.Dot(_rigidbody.linearVelocity, forward);
            if (Mathf.Abs(_direction) > 0.01f)
            {
                if (Mathf.Abs(speed) < _maxSpeed || Mathf.Sign(_direction) - Mathf.Sign(speed) > 0.01f)
                {
                    var force = _direction * _engineForce;
                    _rigidbody.AddForce(forward * force, ForceMode.Force);
                }
            }

            speed = Vector3.Dot(_rigidbody.linearVelocity, forward);

            if (_rearWheels != null && _wheelBase > 0.001f)
            {
                var steerRad = _rearWheels.Angle * Mathf.Deg2Rad;

                var yawRate = 0f;

                if (Mathf.Abs(steerRad) > 0.001f && Mathf.Abs(speed) > 0.01f)
                {
                    yawRate = speed / _wheelBase * Mathf.Tan(steerRad) * _steerFactor;

                    if (_maxYawRate > 0f)
                        yawRate = Mathf.Clamp(yawRate, -_maxYawRate, _maxYawRate);
                }

                var angVel = _rigidbody.angularVelocity;
                var currentUpComponent = Vector3.Dot(angVel, up);

                angVel -= up * currentUpComponent;
                angVel += up * yawRate;

                _rigidbody.angularVelocity = angVel;
            }
        }
    }
}