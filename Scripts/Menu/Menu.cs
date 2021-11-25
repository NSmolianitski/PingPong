using UnityEngine;

namespace PingPong.Menu
{
    public abstract class Menu : MonoBehaviour
    {
        public virtual void OnBackButtonPressed()
        {
            MenuManager.Instance.CloseMenu();
        }
    }
    
    public abstract class Menu <T> : Menu where T : Menu<T>
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            Instance = (T) this;
        }

        protected void OnDestroy()
        {
            Instance = null;
        }
    }
}