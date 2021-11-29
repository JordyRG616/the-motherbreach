using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{

    public void DestroyManager()
    {
        Destroy(this);
    }

}
