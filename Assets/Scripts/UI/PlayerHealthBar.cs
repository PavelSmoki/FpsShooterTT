using FpsShooter.Character;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FpsShooter.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;

        private IPlayer _player;

        [Inject]
        private void Construct(IPlayer player)
        {
            _player = player;
        }

        private void Start()
        {
            _healthBar.maxValue = _player.CurrentHealth.Value;
            _healthBar.value = _player.CurrentHealth.Value;
            _player.CurrentHealth.Subscribe(_ => UpdateHealthBar());
        }

        private void UpdateHealthBar()
        {
            _healthBar.value = _player.CurrentHealth.Value;
        }
    }
}
