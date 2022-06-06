using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretVFXManager : VFXManager
{
    
    [SerializeField] private Color invalidSelectionColor;
    [SerializeField] private Color validSelectionColor;
    private bool activeSelection;

    public void EnableSelected()
    {
        SetSelectedColor(false);
        instMaterial.SetFloat("_Selected", 1f);
    }

    public void DisableSelected()
    {
        instMaterial.SetFloat("_Selected", 0f);
    }

    public void SetSelectedColor(bool selected)
    {
        activeSelection = !selected;

        if(selected == false)
        {
            instMaterial.SetColor("_SelectedColor", invalidSelectionColor);
        } else
        {
            instMaterial.SetColor("_SelectedColor", validSelectionColor);
        }
    }

    public void InitiateBuild()
    {
        StartCoroutine(Build());
    }

    private IEnumerator Build()
    {
        float step = 0;

        while (step <= 1f)
        {
            instMaterial.SetFloat("_Build", step / 1f);
            step += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    public IEnumerator TakeDamage()
    {
        instMaterial.SetFloat("_Damaged", 1f);
        yield return new WaitForSeconds(.05f);
        instMaterial.SetFloat("_Damaged", 0f);
        StopCoroutine(TakeDamage());
    }

    public IEnumerator Die(GameObject dyingObject)
    {
        float step = 0;
        deathParticles.Play();


        Time.timeScale = 0.5f;

        audioManager.RequestSFX(deathSFX);

        while(step <= 1)
        {
            instMaterial.SetFloat("_Decay", step);
            step += 0.01f;
            yield return new WaitForSecondsRealtime(.01f);

            if(Mathf.Approximately(step, .7f))
            {
                Time.timeScale = 1f;
                Destroy(dyingObject);
            }
        }
        

    }

    void Update()
    {
        if(activeSelection && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            audioManager.PlayInvalidSelection("");
        }
    }
}
