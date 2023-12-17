using System.Collections.Generic;
using FpsShooter.Character;
using FpsShooter.Enemies;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace FpsShooter.UI
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Image _winScreen;
        [SerializeField] private Image _loseScreen;
        [SerializeField] private TextMeshProUGUI _winsLabel;
        [SerializeField] private TextMeshProUGUI _losesLabel;

        [Header("Inventory")] [SerializeField] private List<Image> _weaponImages;
        [SerializeField] private List<TextMeshProUGUI> _weaponAmmoLabels;


        private IPlayer _player;
        private SaveSystem _saveSystem;
        private LevelController _levelController;

        [Inject]
        private void Construct(IPlayer player, SaveSystem saveSystem, LevelController levelController)
        {
            _player = player;
            _saveSystem = saveSystem;
            _levelController = levelController;
        }

        private void Start()
        {
            InitHealthBar();
            InitWinLoseLabels();
            InitInventoryUI();

            _levelController.OnAllEnemiesKilled += ShowWinScreen;
            _player.CurrentHealth.Subscribe(_ => ShowLoseScreen());
            _player.OnShoot += UpdateAmmoInfo;
        }

        private void InitHealthBar()
        {
            _healthBar.maxValue = _player.CurrentHealth.Value;
            _healthBar.value = _player.CurrentHealth.Value;
            _player.CurrentHealth.Subscribe(_ => UpdateHealthBar());
        }

        private void InitWinLoseLabels()
        {
            _winsLabel.text = $"Wins: {_saveSystem.WinCount}";
            _losesLabel.text = $"Loses: {_saveSystem.LoseCount}";
        }

        private void InitInventoryUI()
        {
            for (int i = 0; i < _weaponImages.Count; i++)
            {
                UpdateAmmoInfo(i);
            }
        }

        private void UpdateAmmoInfo(int index)
        {
            var inventory = _player.GetInventory();
            
            _weaponImages[index].sprite = inventory.Guns[index].GunSprite;
            _weaponAmmoLabels[index].text = $"{inventory.Guns[index].AmmoLeft}/{inventory.Guns[index].MaxAmmo}";
        }

        private void ShowWinScreen()
        {
            _winScreen.gameObject.SetActive(true);
            _saveSystem.CountWin();
            _winsLabel.text = $"Wins: {_saveSystem.WinCount}";
            Pause();
        }

        private void ShowLoseScreen()
        {
            if (_player.CurrentHealth.Value == 0)
            {
                _loseScreen.gameObject.SetActive(true);
                _saveSystem.CountLose();
                _losesLabel.text = $"Loses: {_saveSystem.LoseCount}";
                Pause();
            }
        }

        private void Pause()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
        }

        private void UpdateHealthBar()
        {
            _healthBar.value = _player.CurrentHealth.Value;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1;
        }
    }
}