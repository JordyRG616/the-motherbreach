using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBox : MonoBehaviour
{
    [SerializeField] private Image sprite;
    [SerializeField] private Image outline;
    [SerializeField] private TextMeshProUGUI description;

    public void SetupBox(EnemyInformation info)
    {
        sprite.sprite = info.enemySprite;
        this.outline.sprite = info.enemyOutline;
        this.description.text = info.enemyDescription;
        gameObject.SetActive(true);
    }

    public void DeactivateBox()
    {
        sprite.sprite = null;
        outline.sprite = null;
        description.text = "";
        gameObject.SetActive(false);
    }
}
