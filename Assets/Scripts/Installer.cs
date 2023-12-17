using FpsShooter.Character;
using FpsShooter.Enemies;
using UnityEngine;
using Zenject;

namespace FpsShooter
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Camera _camera;
        [SerializeField] private LevelController _levelController;
        
        public override void InstallBindings()
        {
            Container.Bind<IPlayer>().FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();
            Container.Bind<Camera>().FromInstance(_camera);
            Container.BindInterfacesAndSelfTo<SaveSystem>().AsSingle().NonLazy();
            Container.Bind<LevelController>().FromInstance(_levelController);
        }
    }
}
