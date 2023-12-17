using System;
using UnityEngine;

namespace FpsShooter.Enemies
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private GameObject _enemiesObj;
        
        private int _enemiesCount;

        public Action OnAllEnemiesKilled;

        private void Start()
        {
            _enemiesCount = _enemiesObj.transform.childCount;
        }

        public void OnEnemyKilled()
        {
            _enemiesCount--;
            if (_enemiesCount == 0)
            {
                OnAllEnemiesKilled.Invoke();
            }
        }
    }
}