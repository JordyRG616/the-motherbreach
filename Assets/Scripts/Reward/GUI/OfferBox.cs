using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfferBox : MonoBehaviour
{
    [SerializeField] private Image TopImage;
    [SerializeField] private Image baseImage;
    [SerializeField] public bool Empty {get; private set;} = true;
    private GameObject turretInOffer;

    public void ReceiveTurret(GameObject turret)
    {
        turretInOffer = turret;
        SpriteRenderer[] spriteRenderers = turret.GetComponentsInChildren<SpriteRenderer>(true);
        
        TopImage.sprite = spriteRenderers[2].sprite;
        TopImage.color = spriteRenderers[2].color;
        baseImage.sprite = spriteRenderers[1].sprite;
        baseImage.color = spriteRenderers[1].color;

        turret.transform.position = transform.position;

        Empty = false;
    }
}
