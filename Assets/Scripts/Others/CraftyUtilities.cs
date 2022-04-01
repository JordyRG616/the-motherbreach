using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftyUtilities 
{
    public static class Utilities
    {
        public static int TestKey(KeyCode key)
        {
            if(Input.GetKey(key) == true)
            {
                return 1;
            } else
            {
                return 0;
            }
        }

        public static void FollowMouse(this RectTransform rect)
        {
            var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f);
            mousePos.x *= 1280;
            mousePos.y *= 720;
            rect.anchoredPosition = mousePos;
        }
    }

}
   
