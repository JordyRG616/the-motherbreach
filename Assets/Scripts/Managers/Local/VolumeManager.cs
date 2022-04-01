using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeManager : MonoBehaviour
{
    public GameState State 
    {
        get
        {
            return activeState;
        }
    }
    [SerializeField] private GameState activeState;
    [SerializeField] private Volume volume;

    private void Active(bool state)
    {
        volume.enabled = state;
    }
}
