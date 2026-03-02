using System;
using UnityEngine;

namespace Forklift.Components
{
    public interface IRearWheelsController
    {
        public float Angle { get; }

        public event Action<Vector3> OnDirectionChanged;
    }

    public class RearWheelsController : MonoBehaviour, IRearWheelsController
    {
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _maxAngle;

        private Vector3 _direction = Vector3.forward;

        public float Angle { get; private set; }

        public event Action<Vector3> OnDirectionChanged;

        public void Rotate(float direction)
        {
            var deltaAngle = direction * _rotateSpeed * Time.deltaTime;
            var newAngle = Mathf.Clamp(Angle + deltaAngle, -_maxAngle, _maxAngle);

            if (Mathf.Approximately(newAngle, Angle))
                return;

            Angle = newAngle;
            RecomputeDirection();
        }

        private void RecomputeDirection()
        {
            _direction = Quaternion.AngleAxis(Angle, Vector3.up) * Vector3.forward;
            OnDirectionChanged?.Invoke(_direction);
        }

        private void OnDestroy()
        {
            OnDirectionChanged = null;
        }
    }
}