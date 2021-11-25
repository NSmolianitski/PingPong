using UnityEngine;
using UnityEngine.UI;

namespace PingPong.Game
{
    public class SkinWindow : MonoBehaviour
    {
        [SerializeField] private Image currentSkinImage;

        private Sprite[] _skins;
        private int _currentSkinIndex = 0;

        private void Start()
        {
            _skins = SkinManager.Instance.Skins;
            currentSkinImage.sprite = _skins[0];
        }

        public void OnNextSkinButtonClicked()
        {
            ++_currentSkinIndex;
            if (_currentSkinIndex == _skins.Length)
                _currentSkinIndex = 0;
            currentSkinImage.sprite = _skins[_currentSkinIndex];
        }
        
        public void OnPreviousSkinButtonClicked()
        {
            --_currentSkinIndex;
            if (_currentSkinIndex == -1)
                _currentSkinIndex = _skins.Length - 1;
            currentSkinImage.sprite = _skins[_currentSkinIndex];
        }

        public void OnSubmitButtonClicked()
        {
            SkinManager.Instance.SetSkin(_currentSkinIndex);
            gameObject.SetActive(false);
        }
    }
}