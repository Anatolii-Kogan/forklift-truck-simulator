using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Forklift.Player
{
    public class ForkliftTruckInputController : MonoBehaviour
    {
        [SerializeField] private ForkliftTruckController _forkliftTruck;

        private GameInputActions _input;

        [Inject]
        private void Contract(GameInputActions input)
        {
            _input = input;
            _input.ForkliftTruck.EngineStart.performed += SwitchEngine;
        }

        private void OnDisable()
        {
            if (_input == null)
                return;

            _input.ForkliftTruck.EngineStart.performed -= SwitchEngine;
        }

        private void Update()
        {
            if (_input != null)
                ReadMovement();
        }

        private void SwitchEngine(InputAction.CallbackContext obj)
        {
            _forkliftTruck.SwitchEngine();
        }

        private void ReadMovement()
        {
            var trackMovement = _input.ForkliftTruck.TrackMove.ReadValue<Vector2>();
            _forkliftTruck.MoveTruck(trackMovement);

            var forkliftMovement = _input.ForkliftTruck.ForkliftMove.ReadValue<float>();
            _forkliftTruck.MoveForklift(forkliftMovement);
        }
    }
}