using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWiggler : MonoBehaviour
{
    private int wiggleAmount;
    private Vector3 wiggleDirection = new Vector3();

    public event EventHandler OnWigglePeak;

    public void StartWiggling(Vector3 direction)
    {
        wiggleDirection += direction;
        if(IsInvoking())
        {
            CancelInvoke();
        }
        InvokeRepeating("Wiggle", 0, .1f);
    }

    private void Wiggle()
    {
        transform.localPosition += wiggleDirection * .1f;
        wiggleAmount++;
        if(wiggleAmount == 16)
        {
            wiggleDirection *= -1;
            OnWigglePeak?.Invoke(this, EventArgs.Empty);
        }
        if(wiggleAmount == 32)
        {
            wiggleDirection *= -1;
            wiggleAmount = 0;
        }
    }
}
