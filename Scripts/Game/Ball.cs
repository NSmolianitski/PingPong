using Photon.Pun;
using PingPong.Game.UserInterface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PingPong.Game
{
    public class Ball : MonoBehaviour, IPunObservable
    {
        [SerializeField] private float startSpeed = 4f;
        [SerializeField] private float speedKoef = 0.5f;
        [SerializeField] private float tiltAngleStrength = 2f;
        [SerializeField] private GameObject hitEffect;

        private float _currentSpeed;
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private PhotonView _photonView;
        
        private Vector2 _networkPosition;

        private const string VictoryZoneTag = "VictoryZone";

        [SerializeField] private bool isTesting = false;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _photonView = GetComponent<PhotonView>();

            int y = 1;
            if (Random.Range(0, 2) == 1)
                y = -1;
            _direction = Vector2.down;
            // _direction = new Vector2(Random.Range(0f, 1f), y);
            _rigidbody.velocity = _direction * startSpeed;
            _currentSpeed = startSpeed;
        }

        private void FixedUpdate()
        {
            if (!PhotonNetwork.IsMasterClient && !isTesting)
            {
                _rigidbody.position = Vector2.MoveTowards(_rigidbody.position, _networkPosition, Time.fixedDeltaTime);
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!PhotonNetwork.IsMasterClient && !isTesting)
                return;
            
            _photonView.RPC(nameof(SpawnHitEffect), RpcTarget.All);
            SoundManager.Instance.PlayBallHitSoundRPC();
            
            if (col.gameObject.CompareTag("Player"))
            {
                Vector2 paddleCenter = col.transform.position;
                var hitPoint = col.GetContact(0).point;

                Vector2 newVelocity = new Vector2((hitPoint.x - paddleCenter.x) * tiltAngleStrength, _rigidbody.velocity.y);
                _rigidbody.velocity = newVelocity.normalized * _currentSpeed;
                _currentSpeed += speedKoef;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            
            SoundManager.Instance.PlayBallOutOfTheFieldSoundRPC();

            if (other.CompareTag(VictoryZoneTag))
            {
                var zone = other.GetComponent<VictoryZone>();
                PlayerUI.Instance.Score.PhotonView.RPC(nameof(PlayerUI.Instance.Score.IncreasePlayerScore), 
                    RpcTarget.All, zone.Player);
                MatchManager.Instance.SendCountdownRPC();
                PhotonNetwork.Destroy(gameObject);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_rigidbody.position);
                stream.SendNext(_rigidbody.velocity);
            }
            else
            {
                _networkPosition = (Vector2) stream.ReceiveNext();
                _rigidbody.velocity = (Vector2) stream.ReceiveNext();
                
                float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
                _networkPosition += _rigidbody.velocity * lag;
            }
        }

        [PunRPC]
        public void SpawnHitEffect()
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }
}