using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    #region Singleton
    private static ShipManager _instance;
    public static ShipManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ShipManager>();
            }
            return _instance;
        }
    }
    #endregion


    
}
