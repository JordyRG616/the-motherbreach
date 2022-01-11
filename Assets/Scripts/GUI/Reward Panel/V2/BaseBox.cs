using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseBox : MonoBehaviour, IPointerClickHandler
{
    private GameObject cachedBase;
    [SerializeField] private Image image;
    private BuildBox buildBox;

    void Start()
    {
        buildBox = FindObjectOfType<BuildBox>();
    }

    public void GenerateNewBase(RewardLevel level)
    {
        cachedBase = TurretConstructor.Main.GetBase(level);
        var sprite = cachedBase.GetComponent<SpriteRenderer>().sprite;
        image.sprite = sprite;
        image.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buildBox.ReceiveBase(cachedBase, this);
    }

    public void Detach()
    {
        cachedBase = null;
        image.color = Color.clear;
    }

    public void Clear()
    {
        Destroy(cachedBase);
        image.color = Color.clear;
    }
}
