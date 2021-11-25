using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace PingPong.Game.UserInterface
{
    public class Score : MonoBehaviour
    {
        public int PlayerOneScore { get; private set; } = 0;
        public int PlayerTwoScore { get; private set; } = 0;
        public PhotonView PhotonView { get; private set; }
        
        private TextMeshProUGUI _scoreText;
        
        public delegate void VictoryHandler(bool isMasterClientWin);
        public event VictoryHandler VictoryEvent;

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            _scoreText = GetComponent<TextMeshProUGUI>();
        }

        public void Reset()
        {
            PlayerOneScore = 0;
            PlayerTwoScore = 0;
            _scoreText.text = ToString();
        }

        [PunRPC]
        public void IncreasePlayerScore(Player player)
        {
            int pointsForVictory = MatchManager.Instance.PointsForVictory;
            if (player.IsMasterClient)
            {
                ++PlayerOneScore;
                if (PlayerOneScore == pointsForVictory)
                    VictoryEvent?.Invoke(true);
            }
            else
            {
                ++PlayerTwoScore;
                if (PlayerTwoScore == pointsForVictory)
                    VictoryEvent?.Invoke(false);
            }

            _scoreText.text = ToString();
        }

        public override string ToString()
        {
            return $"{PlayerOneScore} : {PlayerTwoScore}";
        }
    }
}