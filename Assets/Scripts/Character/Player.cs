using System;
using FpsShooter.Enemies;
using UniRx;
using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;

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

        public IReactiveProperty<int> CurrentHealth { get; } = new ReactiveProperty<int>(0);

        private float _horizontalInput;
        private float _verticalInput;
        private bool _isGrounded;
        private float _actualMoveSpeed;

        private Camera _camera;

        [Inject]
        private void Construct(Camera camera)
        {
            _camera = camera;
        }

        private void Awake()
        {
            CurrentHealth.Value = _maxHealth;
        }

        private void Update()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.2f,
                LayerMask.GetMask(GroundLayerName));

            _rb.drag = _isGrounded ? 1f : 0f;

            PlayerInput();
            PlayerJump();
            SpeedControl();
            Shoot();
        }

        private void Shoot()
        {
            if (Input.GetMouseButton(0))
            {
                var ray = _camera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f,
                    _camera.nearClipPlane));
                if (Physics.Raycast(ray, out var hitInfo, 1000, LayerMask.GetMask(EnemyLayerName, GroundLayerName)))
                {
                    if (hitInfo.transform.gameObject.TryGetComponent<Enemy>(out var enemy))
                        enemy.TakeDamage(0.01f);
                }
            }
        }

        private void PlayerInput()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");

            _actualMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : _moveSpeed;
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
    }
}