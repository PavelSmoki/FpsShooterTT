using UnityEngine;
using UnityEngine.SceneManagement;

namespace FpsShooter.UI
{
    public class MainMenuController : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Game");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
