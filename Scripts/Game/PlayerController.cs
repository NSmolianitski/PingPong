using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace PingPong.Game
{
    public class PlayerController : MonoBehaviour, IPunInstantiateMagicCallback
    {
        [SerializeField] private float speed = 100f;
        
        [SerializeField] private Animator animator;
        private static readonly int Hit = Animator.StringToHash("Hit");
        
        private Rigidbody2D _rigidbody;
        private PhotonView _photonView;
        private int _controlsInversion = 1;

        [SerializeField] private bool isTesting = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _photonView = GetComponent<PhotonView>();
            GameManager.Instance.PlayerLeftEvent += OnPlayerLeft;
            
            if (!_photonView.IsMine && !isTesting)
            {
                Destroy(_rigidbody);
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            object[] data = info.photonView.InstantiationData;
            var isInvertedControls = (bool) data[0];
            _controlsInversion = (isInvertedControls) ? -1 : 1;
        }

        private void Update()
        {
            // InputManager.ReadInput(); // FOR TESTING
            
            if (!_photonView.IsMine && !isTesting)
                return;
            
            InputManager.GetDirection();
        }

        private void FixedUpdate()
        {
            if (!_photonView.IsMine && !isTesting)
                return;
            
            Move();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!PhotonNetwork.IsMasterClient && !isTesting)
                return;
            
            if (other.gameObject.CompareTag("Ball"))
                _photonView.RPC(nameof(SetHitAnimationTrigger), RpcTarget.All);
        }

        private void Move()
        {
            _rigidbody.velocity = InputManager.Direction * (speed * Time.fixedDeltaTime * _controlsInversion);
        }

        public void ChangeSkin(int skinId)
        {
            _photonView.RPC(nameof(SetNewSkin), RpcTarget.AllBuffered, skinId);
        }

        public void OnPlayerLeft(Player player)
        {
            if (!player.IsLocal)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        [PunRPC]
        public void SetHitAnimationTrigger()
        {
            animator.SetTrigger(Hit);
        }
        
        [PunRPC]
        public void SetNewSkin(int skinId)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = SkinManager.Instance.Skins[skinId];
        }
    }
}