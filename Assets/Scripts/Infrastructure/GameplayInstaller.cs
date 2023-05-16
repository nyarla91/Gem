using Gameplay;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private MainCamera _mainCamera;

        public override void InstallBindings()
        {
            Container.Bind<MainCamera>().FromInstance(_mainCamera).AsSingle();
        }
    }
}