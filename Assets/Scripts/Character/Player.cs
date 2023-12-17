using System;
using FpsShooter.Enemies;
using FpsShooter.Guns;
using UniRx;
using UnityEngine;
using Zenject;

namespace FpsShooter.Character
{
    public class Player : MonoBehaviour, IPlayer
    {
        private const string GroundLayerName = "Ground";
        private const string EnemyLayerName = "Enemy";

        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _lookAt;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _sprintSpeed;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private int _maxHealth;
        [SerializeField] private Inventory _inventory;

        public IReactiveProperty<int> CurrentHealth { get; } = new ReactiveProperty<int>(0);
        public Action<int> OnShoot { get; set; }

        private float _horizontalInput;
        private float _verticalInput;
        private bool _isGrounded;
        private float _actualMoveSpeed;
        private Camera _mainCam;

        private Gun _currentWeapon;
        private float _shootDelay;
        private int _currentWeaponIndex;

        [Inject]
        private void Construct(Camera mainCam)
        {
            _mainCam = mainCam;
        }

        private void Awake()
        {
            CurrentHealth.Value = _maxHealth;
            ChangeWeapon(0);
            
            foreach (var gun in _inventory.Guns)
            {
                gun.ResetAmmoLeft();
            }
        }

        private void Update()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.2f,
                LayerMask.GetMask(GroundLayerName));

            _rb.drag = _isGrounded ? 1f : 0f;

            PlayerInput();
            PlayerJump();
            SpeedControl();
            ShootingProcessing();
        }

        private void PlayerInput()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeWeapon(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeWeapon(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeWeapon(2);
            }

            _actualMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : _moveSpeed;
        }

        private void ChangeWeapon(int index)
        {
            _currentWeaponIndex = index;
            _currentWeapon = _inventory.Guns[index];
            _shootDelay = 1 / _currentWeapon.ShootingSpeed;
        }

        private void PlayerJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _rb.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
            }
        }

        private void SpeedControl()
        {
            var flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if (flatVel.magnitude > _actualMoveSpeed)
            {
                var limitedVel = flatVel.normalized * _actualMoveSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }

        private void ShootingProcessing()
        {
            _shootDelay -= Time.deltaTime;

            if (_shootDelay <= 0)
            {
                if (Input.GetMouseButton(0))
                {
                    _shootDelay = 1 / _currentWeapon.ShootingSpeed;
                    Shot();
                }
            }
        }

        private void Shot()
        {
            if (_currentWeapon.AmmoLeft > 0)
            {
                Debug.Log("Shot");
                _currentWeapon.AmmoLeft--;
                OnShoot.Invoke(_currentWeaponIndex);

                var ray = _mainCam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f,
                    _mainCam.nearClipPlane));
                if (Physics.Raycast(ray, out var hitInfo, 1000, LayerMask.GetMask(EnemyLayerName, GroundLayerName)))
                {
                    hitInfo.transform.gameObject.GetComponentInParent<Enemy>()?.TakeDamage(_currentWeapon.Damage);
                }
            }
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }

        private void PlayerMove()
        {
            var moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;

            _rb.AddForce(moveDirection.normalized * (_actualMoveSpeed * 10f), ForceMode.Force);
        }

        public void TakeDamage()
        {
            CurrentHealth.Value--;
        }

        public Transform GetCurrentTransform()
        {
            return transform;
        }

        public Transform GetLookAtTransform()
        {
            return _lookAt;
        }

        public Inventory GetInventory()
        {
            return _inventory;
        }
    }
}