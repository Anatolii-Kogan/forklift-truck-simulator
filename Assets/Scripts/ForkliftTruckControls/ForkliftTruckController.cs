using Forklift.Components;
using Forklift.Displayers;
using UnityEngine;

namespace Forklift.Player
{
    public class ForkliftTruckController : MonoBehaviour
    {
        [SerializeField] private ForkliftTruckMoveController _moveController;
        [SerializeField] private ForkliftMoveController _forkliftController;
        [SerializeField] private RearWheelsController _rearWheels;

        [SerializeField] private WheelsRotationDisplayer[] _wheelsRotationDisplayers;
        [SerializeField] private EngineActiveDisplayer _engineDisplayer;

        private bool _isEngineActive;

        private void Start()
        {
            _moveController.Init(_rearWheels, _rearWheels.transform.position);

            foreach (var displayer in _wheelsRotationDisplayers)
            {
                _rearWheels.OnDirectionChanged += displayer.LookDirection;
            }

            SwitchEngine(_isEngineActive);
        }

        public void SwitchEngine()
        {
            SwitchEngine(!_isEngineActive);
        }

        public void MoveTruck(Vector2 direction)
        {
            _rearWheels.Rotate(direction.x);
            if (!_isEngineActive)
                return;

            _moveController.Move(direction.y);
        }

        public void MoveForklift(float direction)
        {
            _forkliftController.Move(direction);
        }

        private void SwitchEngine(bool isActive)
        {
            _isEngineActive = isActive;
            _moveController.Move(0f);
            _engineDisplayer.Switch(isActive);
        }

        private void OnDestroy()
        {
            foreach (var displayer in _wheelsRotationDisplayers)
            {
                if (_rearWheels == null)
                    break;

                if (displayer == null)
                    continue;

                _rearWheels.OnDirectionChanged -= displayer.LookDirection;
            }
        }
    }
}