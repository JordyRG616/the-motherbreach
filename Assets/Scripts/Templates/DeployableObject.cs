using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DeployableObject : MonoBehaviour
{
    [SerializeField] private float lifetime;
    private WaitForSeconds waitForLifetime;

    public EventHandler onBirth;
    public EventHandler onLifetimeEnd;


    public void Born()
    {
        waitForLifetime = new WaitForSeconds(lifetime);
        gameObject.SetActive(true);
        
        StartCoroutine(Life());
    }



    private IEnumerator Life()
    {
        onBirth?.Invoke(this, EventArgs.Empty);

        yield return waitForLifetime;

        onLifetimeEnd?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(false);
    }
}
