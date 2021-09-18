using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWiggler : MonoBehaviour
{
    private int wiggleAmount;
    private Vector3 wiggleDirection = new Vector3();
    private bool wiggling;

    public event EventHandler OnWigglePeak;

    public void StartWiggling(WigglePattern pattern)
    {
        wiggling = true;
        StartCoroutine(pattern.ToString());
    }

    public void StopWiggling()
    {

    }

    private IEnumerator HorizontalSine()
    {
        while(wiggling)
        {
            float shift = Mathf.Sin(wiggleAmount * .1f * Mathf.Rad2Deg);
            transform.localPosition += Vector3.right * shift;
            wiggleAmount += 1;
            yield return new WaitForSecondsRealtime(.1f);
        }
    }

    private IEnumerator HorizontalCosine()
    {
        while(wiggling)
        {
            float shift = Mathf.Cos((wiggleAmount * .1f * Mathf.Rad2Deg) + 180);
            transform.localPosition += Vector3.right * shift;
            wiggleAmount += 1;
            yield return new WaitForSecondsRealtime(.1f);
        }
    }

    // private void Wiggle()
    // {
    //     transform.localPosition += wiggleDirection * .1f;
    //     wiggleAmount++;
    //     if(wiggleAmount == 16)
    //     {
    //         wiggleDirection *= -1;
    //         OnWigglePeak?.Invoke(this, EventArgs.Empty);
    //     }
    //     if(wiggleAmount == 32)
    //     {
    //         wiggleDirection *= -1;
    //         wiggleAmount = 0;
    //     }
    // }
}
