using UnityEngine;

namespace FpsShooter.Player
{
    public class ThirdPersonCam : MonoBehaviour
    {
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            
        }
    }
}