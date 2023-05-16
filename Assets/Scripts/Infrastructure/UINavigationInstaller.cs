using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class UINavigationInstaller : MonoInstaller
    {
        [SerializeField] private UINavigationSystem _instance;

        public override void InstallBindings()
        {
            Container.Bind<UINavigationSystem>().FromInstance(_instance).AsSingle();
        }
    }
}