using UnityEngine;

namespace Forklift.Displayers
{
    public class EngineActiveDisplayer : MonoBehaviour
    {
        [SerializeField] private Material _switchedOnMaterial;
        [SerializeField] private Material _switchedOffMaterial;

        [SerializeField] private MeshRenderer _target;

        public void Switch(bool isActive)
        {
            _target.material = isActive ? _switchedOnMaterial : _switchedOffMaterial;
        }
    }
}