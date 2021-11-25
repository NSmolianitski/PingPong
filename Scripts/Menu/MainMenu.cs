using UnityEngine;
using UnityEngine.UI;

namespace PingPong.Menu
{
    public class MainMenu : Menu<MainMenu>
    {
        [SerializeField] private Button playButton;

        public void EnablePlayButton()
        {
            playButton.interactable = true;
        }
        
        public void DisablePlayButton()
        {
            playButton.interactable = false;
        }
        
        public override void OnBackButtonPressed()
        {
            Application.Quit();
        }

        public void OnPlayButtonClicked()
        {
            GameManager.Instance.JoinRandomRoom();
        }
        
        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}

