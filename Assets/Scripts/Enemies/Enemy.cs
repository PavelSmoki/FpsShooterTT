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
        [SerializeField] private float _attackTime;
        
        public IReactiveProperty<float> CurrentHealth { get; } = new ReactiveProperty<float>(0f);

        private IPlayer _player;
        private LevelController _levelController;
        private Transform _playerTransform;
        private float _attackDelay;

        [Inject]
        private void Construct(IPlayer player, LevelController levelController)
        {
            _player = player;
            _levelController = levelController;
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
                _levelController.OnEnemyKilled();
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            _attackDelay += Time.deltaTime;

            if (_attackDelay >= _attackTime)
            {
                if (other.CompareTag("Player"))
                {
                    _attackDelay = 0;
                    other.gameObject.GetComponent<Player>().TakeDamage();
                }
            }
        }
    }
}