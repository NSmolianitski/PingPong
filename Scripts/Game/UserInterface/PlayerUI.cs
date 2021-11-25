using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PingPong.Game.UserInterface
{
    public class PlayerUI : MonoBehaviour
    {
        public static PlayerUI Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [SerializeField] private GameMessage message;
        [SerializeField] private Score score;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button leaveButton;
        [SerializeField] private Button skinChangeButton;
        [SerializeField] private Countdown countdown;

        public Score Score => score;
        public Countdown Countdown => countdown;

        private void Start()
        {
            if (GameManager.Instance.OtherPlayerLeft)
                message.SetOtherPlayerLeftMessage();
            MatchManager.Instance.PlayerIsReadyEvent += SetOnePlayerReadinessText;
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2) // Called when second player joined (for second player)
                SetCheckReadinessUI();
        }

        private void SetOnePlayerReadinessText()
        {
            if (MatchManager.Instance.ReadyPlayers == 1)
                message.SetOnePlayerReady();
        }

        public void ResetUI()
        {
            SwitchButtons(true);
            readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "READY!";
            ResetMessage();
            ResetScore();
        }
        
        public void Restart()
        {
            SwitchButtons(true);
            readyButton.interactable = false;
            readyButton.interactable = true;
            readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "RESTART?";
            RestartMessage();
            ResetScore();
            message.SetZeroPlayersReady();
        }

        private void ResetScore()
        {
            score.gameObject.SetActive(false);
            score.Reset();
        }

        private void ResetMessage()
        {
            message.gameObject.SetActive(true);
            message.SetOtherPlayerLeftMessage();
        }
        
        private void RestartMessage()
        {
            message.gameObject.SetActive(true);
            message.SetZeroPlayersReady();
        }

        private void SwitchButtons(bool turnOn)
        {
            readyButton.gameObject.SetActive(turnOn);
            skinChangeButton.gameObject.SetActive(turnOn);
            leaveButton.gameObject.SetActive(turnOn);
        }
        
        public void LaunchCountdown()
        {
            message.gameObject.SetActive(false);
            countdown.gameObject.SetActive(true);
            countdown.Launch(true);
        }
        
        public void LaunchRestartCountdown()
        {
            countdown.gameObject.SetActive(true);
            countdown.Launch(false);
        }

        public void SetCheckReadinessUI()
        {
            message.SetZeroPlayersReady();
            SwitchButtons(true);
        }

        public void OnReadyButtonClicked()
        {
            SwitchButtons(false);
            MatchManager.Instance.SendPlayerReadiness();
        }

        public void OnLeaveButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void OnSkinChangeButtonClicked()
        {
            SkinManager.Instance.OpenSkinWindow();
        }

        public void ShowWin()
        {
            score.gameObject.SetActive(false);
            message.gameObject.SetActive(true);
            message.ShowMessageWithText($"You win with score: {Score}");
        }

        public void ShowLose()
        {
            score.gameObject.SetActive(false);
            message.gameObject.SetActive(true);
            message.ShowMessageWithText($"You lose with score: {Score}");
        }

        public void ShowEnemyLeft()
        {
            score.gameObject.SetActive(false);
            message.gameObject.SetActive(true);
            message.ShowMessageWithText("Your opponent has left, congratulations!");
        }
    }
}
