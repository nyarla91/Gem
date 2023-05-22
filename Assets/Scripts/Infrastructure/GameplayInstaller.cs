using Gameplay;
using Gameplay.Character.Player;
using UnityEngine;
using Zenject;
using MonoInstaller = Extentions.MonoInstaller;

namespace Infrastructure
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private MainCamera _mainCamera;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;

        public override void InstallBindings()
        {
            Container.Bind<MainCamera>().FromInstance(_mainCamera).AsSingle();

            GameObject player = Container.InstantiatePrefab(_playerPrefab, _playerSpawnPoint.position,
                _playerSpawnPoint.rotation, null);
            BindFromInstance<IPlayerTransformInfo>(player);
        }
    }
}