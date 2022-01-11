using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWiggler : MonoBehaviour
{
    private int wiggleAmount;
    private bool wiggling;
    private float frequency;
    private Vector3 StartingPosition;


    public void StartWiggling(WigglePattern pattern)
    {
        StartingPosition = transform.localPosition;
        frequency = .1f;
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
            float shift = Mathf.Sin(wiggleAmount * Mathf.Deg2Rad);
            transform.localPosition = new Vector3(shift, 0, 0) + StartingPosition;
            wiggleAmount += 10;
            yield return new WaitForSeconds(frequency);
        }
    }

    private IEnumerator HorizontalCosine()
    {
        while(wiggling)
        {
            float shift = Mathf.Cos((wiggleAmount + 90) * Mathf.Deg2Rad);
            transform.localPosition =  new Vector3(shift, 0, 0) + StartingPosition;
            wiggleAmount += 10;
            yield return new WaitForSeconds(frequency);
        }
    }

    private IEnumerator VerticalSine()
    {
        while(wiggling)
        {
            float shift = Mathf.Sin(wiggleAmount * Mathf.Deg2Rad);
            transform.localPosition = new Vector3(0, shift, 0) + StartingPosition;
            wiggleAmount += 10;
            yield return new WaitForSeconds(frequency);
        }
    }

    private IEnumerator VerticalCosine()
    {
        while(wiggling)
        {
            float shift = Mathf.Cos ((wiggleAmount + 90f) * Mathf.Deg2Rad );
            transform.localPosition = new Vector3(0, shift, 0) + StartingPosition;
            wiggleAmount += 10;
            yield return new WaitForSeconds(frequency);
        }
    }

    private IEnumerator ClockwiseCircle()
    {
        while(wiggling)
        {
            float angle = (wiggleAmount * Mathf.Deg2Rad);
            transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) + StartingPosition;
            wiggleAmount -= 10;
            yield return new WaitForSeconds(frequency);
        }
    }

    private IEnumerator CounterClockwiseCircle()
    {
        while(wiggling)
        {
            float angle = (wiggleAmount * Mathf.Deg2Rad);
            transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle))  + StartingPosition;
            wiggleAmount += 10;
            yield return new WaitForSeconds(frequency);
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
