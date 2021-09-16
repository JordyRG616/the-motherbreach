using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
  
    void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent<ActionEffect>(out ActionEffect action))
        {
            action.ApplyEffect(this);
        }
    }
}
