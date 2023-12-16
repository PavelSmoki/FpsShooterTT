using UnityEngine;

namespace FpsShooter.Player
{
    public class Player : MonoBehaviour
    {
        private const string GroundLayerName = "Ground";
        
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _speed;

        private bool _isGrounded;

        private void Update()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.2f, 
                LayerMask.GetMask(GroundLayerName));

            _rb.drag = _isGrounded ? 1f : 0f;
            
            PlayerJump();
        }

        private void PlayerJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            }
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }

        private void PlayerMove()
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _rb.AddForce(transform.forward * (_speed * 1.3f), ForceMode.Force);
                }
                else
                {
                    _rb.AddForce(transform.forward * _speed, ForceMode.Force);
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                _rb.AddForce(transform.forward * -_speed, ForceMode.Force);
            }

            if (Input.GetKey(KeyCode.A))
            {
                _rb.AddForce(transform.right * -_speed, ForceMode.Force);
            }

            if (Input.GetKey(KeyCode.D))
            {
                _rb.AddForce(transform.right * _speed, ForceMode.Force);
            }
        }
    }
}