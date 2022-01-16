using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoTab : MonoBehaviour
{
    void Start()
    {
        
    }

    public void ShowTab()
    {
        gameObject.SetActive(true);
    }

    public void HideTab()
    {
        gameObject.SetActive(false);
    }
}
