using UnityEngine;

namespace PingPong.Game
{
    public class SkinManager : MonoBehaviour
    {
        public static SkinManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [SerializeField] private SkinWindow skinWindow;
        [SerializeField] private Sprite[] skins;
        public Sprite[] Skins => skins;
        
        public PlayerController LocalPlayer { get; set; }
        
        public void OpenSkinWindow()
        {
            skinWindow.gameObject.SetActive(true);
        }

        public void SetSkin(int skinId)
        {
            LocalPlayer.ChangeSkin(skinId);
        }
    }
}