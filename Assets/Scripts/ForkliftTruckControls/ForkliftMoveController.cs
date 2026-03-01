using UnityEngine;

namespace Forklift.Player
{
    public class ForkliftMoveController : MonoBehaviour
    {
        [Header("Limits relative to Anchor along Parent.Up (meters)")]
        [SerializeField] private float _maxPosition = 1.0f;
        [SerializeField] private float _minPosition = 0.0f;

        [Header("Motion")]
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _spring = 20000f;
        [SerializeField] private float _damper = 2000f;
        [SerializeField] private float _maxForce = 1e6f;

        [Header("Refs")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Rigidbody _connectedBody;
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform _anchor;

        [SerializeField] private ConfigurableJoint _joint;

        private float _direction;
        private float _targetFromAnchor;

        private Vector3 Up => _parent.up;

        private void Awake()
        {
            ConfigureJoint();
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
            if (_joint == null) return;

            _targetFromAnchor = Mathf.Clamp(
                _targetFromAnchor + _direction * _speed * Time.fixedDeltaTime,
                _minPosition,
                _maxPosition
            );

            _joint.targetPosition = new Vector3(0f, -_targetFromAnchor, 0f);
        }

        private void ConfigureJoint()
        {
            _joint.connectedBody = _connectedBody;
            _joint.autoConfigureConnectedAnchor = false;
            _joint.configuredInWorldSpace = false;

            _joint.xMotion = ConfigurableJointMotion.Locked;
            _joint.yMotion = ConfigurableJointMotion.Limited;
            _joint.zMotion = ConfigurableJointMotion.Locked;

            _joint.angularXMotion = ConfigurableJointMotion.Locked;
            _joint.angularYMotion = ConfigurableJointMotion.Locked;
            _joint.angularZMotion = ConfigurableJointMotion.Locked;

            _joint.axis = transform.right;
            _joint.secondaryAxis = Up;

            float mid = 0.5f * (_minPosition + _maxPosition);
            Vector3 midWorld = _anchor.position + Up * mid;

            _joint.anchor = _rigidbody.transform.InverseTransformPoint(midWorld);
            _joint.connectedAnchor = _connectedBody.transform.InverseTransformPoint(midWorld);

            var limit = _joint.linearLimit;
            limit.limit = _maxPosition;
            _joint.linearLimit = limit;

            var yDrive = _joint.yDrive;
            yDrive.positionSpring = _spring;
            yDrive.positionDamper = _damper;
            yDrive.maximumForce = _maxForce;
            _joint.yDrive = yDrive;

            _targetFromAnchor = _minPosition;

            _joint.targetPosition = new Vector3(0f, -_targetFromAnchor, 0f);
        }
    }
}