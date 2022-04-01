using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform firstHealthBar;
    [SerializeField] private RectTransform secondHealthBar;
    [SerializeField] private RectTransform thirdHealthBar;
    [SerializeField] private RectTransform healthBarFill;
    [SerializeField] private ParticleSystem fillVFX;
    [SerializeField] private RectTransform bg;
    [SerializeField] private ParticleSystem secondPhaseMarker;
    [SerializeField] private ParticleSystem thirdPhaseMarker;
    private int maximumHealth;

    public void InitiateHealthBar(int maxHealth, List<float> thresholds)
    {
        var height = firstHealthBar.sizeDelta.y;
        maximumHealth = maxHealth;

        firstHealthBar.sizeDelta = new Vector2(maxHealth / 2 * (thresholds[0] - thresholds[1]), height);
        secondHealthBar.sizeDelta = new Vector2(maxHealth / 2  * (thresholds[1] - thresholds[2]), height);
        thirdHealthBar.sizeDelta = new Vector2(maxHealth / 2 * thresholds[2], height);
        healthBarFill.sizeDelta = new Vector2((maxHealth / 2) - 35, healthBarFill.sizeDelta.y);
        bg.sizeDelta = new Vector2((maxHealth / 2) + 100, bg.sizeDelta.y);

        var animations = firstHealthBar.GetComponentsInParent<UIAnimations>();

        foreach(UIAnimations animation in animations)
        {
            animation.Play();
        }
    }

    public void UpdateHealthBar(float percentage)
    {
        healthBarFill.sizeDelta = new Vector2
            (
                percentage * ((maximumHealth / 2) - 35),
                healthBarFill.sizeDelta.y
            );
        fillVFX.Play();
    }

    public void ActivatePhaseMarker(int phase)
    {
        switch(phase)
        {
            case 1:
                secondPhaseMarker.Play();
            break;
            case 2:
                thirdPhaseMarker.Play();
            break;
        }
    }

    public void TerminateMarkers()
    {
        secondPhaseMarker.Stop();
        thirdPhaseMarker.Stop();

        var animations = firstHealthBar.GetComponentsInParent<UIAnimations>();

            foreach(UIAnimations animation in animations)
            {
                animation.PlayReverse();
            }

    }

}
