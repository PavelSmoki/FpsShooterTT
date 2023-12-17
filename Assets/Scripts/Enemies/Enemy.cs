using FpsShooter.Character;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace FpsShooter.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

        [SerializeField] private float _maxHealth;
        [SerializeField] private float _attackSpeed;
        private float _attackDelay;

        public IReactiveProperty<float> CurrentHealth { get; } = new ReactiveProperty<float>(0f);

        private IPlayer _player;
        private Transform _playerTransform;

        [Inject]
        private void Construct(IPlayer player)
        {
            _player = player;
        }

        private void Awake()
        {
            CurrentHealth.Value = _maxHealth;
            _playerTransform = _player.GetCurrentTransform();
        }

        private void Update()
        {
            _agent.SetDestination(_playerTransform.position);
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth.Value -= damage;
            if (CurrentHealth.Value <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            _attackDelay += Time.deltaTime;
            
            if (_attackDelay >= _attackSpeed)
            {
                _attackDelay = 0;
                other.gameObject.GetComponent<Player>().TakeDamage();
            }
        }
    }
}