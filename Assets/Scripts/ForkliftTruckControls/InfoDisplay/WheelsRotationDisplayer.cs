using UnityEngine;

namespace Forklift.Displayers
{
    public class WheelsRotationDisplayer : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        public void LookDirection(Vector3 direction)
        {
            var localRot = Quaternion.LookRotation(direction, Vector3.up); 
            _target.localRotation = localRot;
        }
    }
}