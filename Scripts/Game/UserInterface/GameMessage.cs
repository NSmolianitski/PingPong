using TMPro;
using UnityEngine;

namespace PingPong.Game.UserInterface
{
    public class GameMessage : MonoBehaviour
    {
        private const string WaitingString = "Waiting for other player...";
        private const string OtherPlayerLeftString = "Your opponent has left. Waiting for a new player...";
        private const string ZeroPlayersReadyString = "0 players are ready.";
        private const string OnePlayerReadyString = "1 player is ready.";

        private TextMeshProUGUI _message;

        private void Awake()
        {
            _message = GetComponent<TextMeshProUGUI>();
            _message.text = WaitingString;
        }

        private void Start()
        {
            MatchManager.Instance.GameStartedEvent += OnGameStarted;
        }

        public void SetZeroPlayersReady()
        {
            _message.text = ZeroPlayersReadyString;
        }
        
        public void SetOnePlayerReady()
        {
            _message.text = OnePlayerReadyString;
        }

        private void OnGameStarted()
        {
            gameObject.SetActive(false);
        }

        public void SetOtherPlayerLeftMessage()
        {
            _message.text = OtherPlayerLeftString;
        }

        public void ShowMessageWithText(string text)
        {
            _message.text = text;
        }
    }
}