using System;
using Photon.Pun;
using PingPong.Game.UserInterface;
using UnityEngine;

namespace PingPong.Game
{
    public class MatchManager : MonoBehaviour
    {
        public static MatchManager Instance { get; private set; }

        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private Ball ballPrefab;
        
        [SerializeField] private Transform playerOneSpawnPoint;
        [SerializeField] private Transform playerTwoSpawnPoint;

        [SerializeField] private VictoryZone playerOneVictoryZone;
        [SerializeField] private VictoryZone playerTwoVictoryZone;

        [SerializeField] private int pointsForVictory = 1;
        public int PointsForVictory => pointsForVictory;

        public bool IsGameEnded { get; private set; } = false;
        
        private PhotonView _photonView;
        
        private void Awake()
        {
            Instance = this;
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            PlayerUI.Instance.Countdown.EndedEvent += CountdownHandler;
            PlayerUI.Instance.Score.VictoryEvent += ShowVictoryScreen;
            SpawnPlayer();
        }

        private void OnDestroy()
        {
            Instance = null;
        }
        
        // Game
        public delegate void PlayerReadyHandler();
        public event PlayerReadyHandler PlayerIsReadyEvent;
        
        public delegate void GameStartHandler();
        public event GameStartHandler GameStartedEvent;

        public byte ReadyPlayers { get; private set; } = 0;

        private GameObject _ballInstance;

        public void SendPlayerReadiness()
        {
            _photonView.RPC(nameof(PlayerIsReady), RpcTarget.All);
        }

        private void CountdownHandler()
        {
            if (!IsGameEnded)
                StartGame();
            else
                _photonView.RPC(nameof(RestartGame), RpcTarget.All);
        }

        private void StartGame()
        {
            PlayerUI.Instance.Score.gameObject.SetActive(true);
            GameStartedEvent?.Invoke();
            if (PhotonNetwork.IsMasterClient)
            {
                _photonView.RPC(nameof(SetVictoryZonePlayers), RpcTarget.All);
                _ballInstance = PhotonNetwork.Instantiate(ballPrefab.name, Vector3.zero, Quaternion.identity);
            }
        }

        public void OnPlayerLeft()
        {
            if (_ballInstance != null)
                Destroy(_ballInstance);
        }

        public void SendCountdownRPC()
        {
            _photonView.RPC(nameof(LaunchCountdown), RpcTarget.All);
        }

        private void ShowVictoryScreen(bool isMasterClientWin)
        {
            IsGameEnded = true;
            if (PhotonNetwork.IsMasterClient)
            {
                if (isMasterClientWin)
                    PlayerUI.Instance.ShowWin();
                else
                    PlayerUI.Instance.ShowLose();
                PlayerUI.Instance.LaunchRestartCountdown();
            }
            else
            {
                if (!isMasterClientWin)
                    PlayerUI.Instance.ShowWin();
                else
                    PlayerUI.Instance.ShowLose();
            }
        }

        [PunRPC]
        public void RestartGame()
        {
            try
            {
            IsGameEnded = false;
            PlayerUI.Instance.Restart();
            ReadyPlayers = 0;
            }
            catch (Exception e)
            {
                Debug.Log("Exception caught!!!!!!!!!!!!!!!");
            }
        }
        
        [PunRPC]
        public void PlayerIsReady()
        {
            ++ReadyPlayers;
            if (ReadyPlayers == 2)
                PlayerUI.Instance.LaunchCountdown();
            else
                PlayerIsReadyEvent?.Invoke();
        }

        private void SetSkinManagerPlayer(GameObject player)
        {
            SkinManager.Instance.LocalPlayer = player.GetComponent<PlayerController>();
        }

        [PunRPC]
        public void SpawnPlayer()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                object[] data = { false }; // inverted controls
                var player = PhotonNetwork.Instantiate(playerPrefab.name, playerOneSpawnPoint.position, 
                    Quaternion.identity, 0, data);
                SetSkinManagerPlayer(player);
            }
            else
            {
                Camera.main.transform.Rotate(0f, 0f, 180f);
                object[] data = { true }; // inverted controls
                var player = PhotonNetwork.Instantiate(playerPrefab.name, playerTwoSpawnPoint.position, 
                    Quaternion.Euler(0f, 0f, 180f), 0, data);
                SetSkinManagerPlayer(player);
            }
        }

        [PunRPC]
        public void SetVictoryZonePlayers()
        {
            playerOneVictoryZone.Player = PhotonNetwork.PlayerList[0];
            playerTwoVictoryZone.Player = PhotonNetwork.PlayerList[1];
        }

        [PunRPC]
        public void LaunchCountdown()
        {
            if (IsGameEnded)
                return;
            PlayerUI.Instance.LaunchCountdown();
        }
    }
}