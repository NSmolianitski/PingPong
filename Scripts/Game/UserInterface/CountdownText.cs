using TMPro;
using UnityEngine;

namespace PingPong.Game.UserInterface
{
    public class CountdownText : MonoBehaviour
    {
        private TextMeshProUGUI _textField;

        private void Awake()
        {
            _textField = GetComponent<TextMeshProUGUI>();
        }

        public void SetText(int time)
        {
            _textField.text = time.ToString();
        }
    }
}