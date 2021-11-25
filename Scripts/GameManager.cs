using Photon.Pun;
using Photon.Realtime;
using PingPong.Game;
using PingPong.Game.UserInterface;
using PingPong.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PingPong
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            MenuManager.Instance.OpenMenu<MainMenu>();
            PhotonNetwork.AutomaticallySyncScene = true;
            
            if (!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
        }

        private void Update()
        {
            InputManager.ReadInput();
        }

        // Connection
        public bool OtherPlayerLeft { get; private set; } = false;
        
        public delegate void PlayerLeftHandler(Player player);
        public event PlayerLeftHandler PlayerLeftEvent;
        
        public override void OnConnectedToMaster()
        {
            MainMenu.Instance.EnablePlayButton();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            MainMenu.Instance.DisablePlayButton();
        }

        public override void OnJoinedRoom()
        {
            MenuManager.Instance.CloseMenu();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }

        public override void OnCreatedRoom()
        {
            PhotonNetwork.LoadLevel(1);
        }

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom(null, 2);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 });
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log(newPlayer + " joined.");
            PlayerUI.Instance.SetCheckReadinessUI();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
                Destroy(PlayerUI.Instance.gameObject);
            OtherPlayerLeft = true;
            Debug.Log(otherPlayer + " left.");
            PlayerUI.Instance.ResetUI();
            MatchManager.Instance.OnPlayerLeft();
            PlayerLeftEvent?.Invoke(otherPlayer);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}