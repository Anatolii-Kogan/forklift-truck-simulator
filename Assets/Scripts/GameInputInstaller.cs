using Zenject;

namespace Forklift.Installers
{
    public class GameInputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var input = new GameInputActions();
            input.Enable();
            Container.Bind<GameInputActions>().FromInstance(input).AsSingle();
        }
    }
}