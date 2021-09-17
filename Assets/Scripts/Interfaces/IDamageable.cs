using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public void UpdateHealth(float amount);
    public void UpdateHealthBar();
    public void DestroyDamageable();
}
