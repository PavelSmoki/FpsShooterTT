using FpsShooter.Character;
using FpsShooter.UI;
using UnityEngine;
using Zenject;

namespace FpsShooter
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerHealthBar _playerHealthBar;
        
        public override void InstallBindings()
        {
            Container.Bind<IPlayer>().FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();
            Container.Bind<Camera>().FromInstance(_camera);
            Container.Bind<PlayerHealthBar>().FromInstance(_playerHealthBar);
        }
    }
}
