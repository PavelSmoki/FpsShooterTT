using FpsShooter.Enemies;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace FpsShooter.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Slider _healthBar;
        
        private void Start()
        {
            _healthBar.maxValue = _enemy.CurrentHealth.Value;
            _healthBar.value = _enemy.CurrentHealth.Value;
            _enemy.CurrentHealth.Subscribe(_ => UpdateHealthBar());
        }

        private void UpdateHealthBar()
        {
            _healthBar.value = _enemy.CurrentHealth.Value;
        }
    }
}
