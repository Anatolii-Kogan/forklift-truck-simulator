using System;
using UnityEngine;

namespace Forklift.Components
{
    public interface IRearWheelsController : ITruckComponent
    {
        public float Angle { get; }
        public Vector3 Direction { get; }

        public event Action<Vector3> OnDirectionChanged;
    }

    public class RearWheelsController : MonoBehaviour, IRearWheelsController
    {
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _maxAngle;

        public float Angle { get; private set; }
        public Vector3 Direction { get; private set; } = Vector3.forward;

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
            Direction = Quaternion.AngleAxis(Angle, Vector3.up) * Vector3.forward;
            OnDirectionChanged?.Invoke(Direction);
        }

        private void OnDestroy()
        {
            OnDirectionChanged = null;
        }
    }
}