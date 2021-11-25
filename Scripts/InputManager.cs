using UnityEngine;

namespace PingPong
{
    public static class InputManager
    {
        public static bool BackButtonPressed = false;
        
        public static bool LeftButtonPressed = false;
        public static bool RightButtonPressed = false;
        public static Vector2 Direction = Vector2.zero;

        private const string HorizontalAxis = "Horizontal";
            
        public static void ReadInput()
        {
            BackButtonPressed = Input.GetKeyDown(ControlButtons.BackKey);
            LeftButtonPressed = Input.GetKeyDown(ControlButtons.LeftKey);
            RightButtonPressed = Input.GetKeyDown(ControlButtons.RightKey);
        }

        public static void GetDirection()
        {
            Direction = new Vector2(Input.GetAxisRaw(HorizontalAxis), 0);
        } 
    }
}