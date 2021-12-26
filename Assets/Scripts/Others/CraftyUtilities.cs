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

    }

}
   
