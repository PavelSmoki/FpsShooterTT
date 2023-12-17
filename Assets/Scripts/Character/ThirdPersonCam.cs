using Cinemachine;
using UnityEngine;
using Zenject;

namespace FpsShooter.Character
{
    public class ThirdPersonCam : MonoBehaviour
    {
        private IPlayer _player;

        [Inject]
        private void Construct(IPlayer player)
        {
            _player = player;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            var cinemachine = gameObject.GetComponent<CinemachineFreeLook>();
            cinemachine.Follow = _player.GetCurrentTransform();
            cinemachine.LookAt = _player.GetLookAtTransform();
        }

        private void Update()
        {
            var lookAt = _player.GetLookAtTransform();
            
            var dirToLookAt = lookAt.position - new Vector3(transform.position.x, 
                lookAt.position.y, transform.position.z);

            _player.GetCurrentTransform().forward = dirToLookAt.normalized;

        }
    }
}