using System;
using System.Collections.Generic;
using UnityEngine;

namespace PingPong.Menu
{
    public sealed class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
                throw new Exception("MenuManager already exists!");
            Instance = this;
        }

        private void Update()
        {
            if (InputManager.BackButtonPressed && _menuStack.Count > 0)
                _menuStack.Peek().OnBackButtonPressed();
        }

        private void OnDestroy()
        {
            Instance = null;
        }
        
        // Menu prefabs
        [SerializeField] private Menu mainMenuPrefab;
        [SerializeField] private Menu ingameMenuPrefab;

        private T GetPrefab<T>() where T : Menu
        {
            if (typeof(T) == typeof(MainMenu))
                return mainMenuPrefab as T;
            else if (typeof(T) == typeof(IngameMenu))
                return ingameMenuPrefab as T;
            
            throw new MissingReferenceException("No such menu type");
        }
        
        // Menu interaction
        private readonly Stack<Menu> _menuStack = new Stack<Menu>();
        
        public void OpenMenu<T>() where T : Menu
        {
            var prefab = GetPrefab<T>();
            var instance = Instantiate(prefab, transform);

            if (_menuStack.Count > 0)
                _menuStack.Peek().gameObject.SetActive(false);
            
            _menuStack.Push(instance);
        }

        public void CloseMenu()
        {
            var instance = _menuStack.Pop();
            Destroy(instance.gameObject);

            if (_menuStack.Count > 0)
                _menuStack.Peek().gameObject.SetActive(true);
        }
    }
}