using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TargetableComponent : MonoBehaviour
{
    public EventHandler OnDetach;

    public void Detach()
    {
        OnDetach?.Invoke(this, EventArgs.Empty);
    }
}
