using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretVFXManager : VFXManager
{
    
    [SerializeField] private Color invalidSelectionColor;
    [SerializeField] private Color validSelectionColor;
    
    
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
        if(selected == false)
        {
            instMaterial.SetColor("_SelectedColor", invalidSelectionColor);
        } else
        {
            instMaterial.SetColor("_SelectedColor", validSelectionColor);
        }
    }

    public IEnumerator TakeDamage()
    {
        instMaterial.SetFloat("_Damaged", 1f);
        yield return new WaitForSecondsRealtime(.05f);
        instMaterial.SetFloat("_Damaged", 0f);
        StopCoroutine(TakeDamage());
    }

    public IEnumerator Die(GameObject dyingObject)
    {
        float step = 0;
        deathParticles.Play();

        audioManager.RequestSFX(deathSFX);

        while(step <= 1)
        {
            instMaterial.SetFloat("_Decay", step);
            step += 0.01f;
            yield return new WaitForSecondsRealtime(.01f);

            if(step >= .6f)
            {
                Destroy(dyingObject);
            }
        }

    }
}
