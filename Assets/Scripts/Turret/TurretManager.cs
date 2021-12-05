using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    
    public BaseEffectTemplate BaseEffect {get; private set;}
    public ActionController ActionController {get; private set;}

    void Start()
    {
        BaseEffect = GetComponentInChildren<BaseEffectTemplate>();
        ActionController = GetComponentInChildren<ActionController>();
    }   
    
}