using Photon.Pun;
using UnityEngine;

namespace PingPong.Game
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
            _photonView = GetComponent<PhotonView>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [SerializeField] private AudioClip ballHitSound;
        [SerializeField] private AudioClip ballOutOfTheFieldSound;
        
        private AudioSource _audioSource;
        private PhotonView _photonView;

        public void PlayBallHitSoundRPC()
        {
            _photonView.RPC(nameof(PlayBallHitSound), RpcTarget.All);
        }
        
        public void PlayBallOutOfTheFieldSoundRPC()
        {
            _photonView.RPC(nameof(PlayBallOutOfTheFieldSound), RpcTarget.All);
        }
        
        [PunRPC]
        public void PlayBallHitSound()
        {
            _audioSource.clip = ballHitSound;
            _audioSource.volume = 1;
            _audioSource.Play();
        }

        [PunRPC]
        public void PlayBallOutOfTheFieldSound()
        {
            _audioSource.clip = ballOutOfTheFieldSound;
            _audioSource.volume = 0.5f;
            _audioSource.Play();
        }
    }
}