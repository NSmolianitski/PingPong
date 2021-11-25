using TMPro;
using UnityEngine;

namespace PingPong.Game.UserInterface
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] private int time = 3;
        [SerializeField] private CountdownText countdownText;
        
        private float _currentTime;

        public bool IsActive { get; private set; } = false;

        public delegate void EndHandler();
        public event EndHandler EndedEvent;
        
        private void Update()
        {
            if (IsActive)
            {
                _currentTime -= Time.deltaTime;
                if (countdownText.enabled)
                    countdownText.SetText((int) _currentTime);
                if (_currentTime < 1)
                {
                    IsActive = false;
                    EndedEvent?.Invoke();
                    gameObject.SetActive(false);
                }
            }
        }

        public void Launch(bool enableText)
        {
            countdownText.gameObject.SetActive(enableText);
            _currentTime = time + 1;
            IsActive = true;
        }
    }
}