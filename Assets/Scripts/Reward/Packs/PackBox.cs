using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PackBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameMesh;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private RectTransform componentsPanel;
    [SerializeField] private GameObject componentBox;
    [SerializeField] private Color weaponColor;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color artifactColor;
    private Pack storagedPack;


    public void ConfigureBox(Pack pack)
    {
        storagedPack = pack;
        nameMesh.text = pack.name;
        iconImage.sprite = pack.icon;
        iconImage.GetComponent<Animator>().SetInteger("Index", pack.index);
        description.text = pack.description;

        foreach(GameObject component in pack.rewards)
        {
            NewComponentBox(component);
        }

        foreach(UIAnimations animation in GetComponents<UIAnimations>())
        {
            animation.Play();
        }
    }

    public void Disable()
    {
        foreach(UIAnimations animation in GetComponents<UIAnimations>())
        {
            animation.PlayReverse();
        }

        iconImage.GetComponent<Animator>().SetTrigger("Reset");

        var components = componentsPanel.GetComponentsInChildren<ComponentBox>();

        foreach(ComponentBox componentBox in components)
        {
            Destroy(componentBox.gameObject);
        }
    }

    private GameObject NewComponentBox(GameObject component)
    {
        Color color;
        Sprite sprite;

        if(component.TryGetComponent<Artifact>(out var artifact))
        {
            color = artifactColor;
            sprite = artifact.icon;
        } else
        {
            color = component.TryGetComponent<ActionController>(out var controller) ? weaponColor : baseColor;
            sprite = component.GetComponent<SpriteRenderer>().sprite;
        } 
        var container = Instantiate(componentBox, Vector3.zero, Quaternion.identity, componentsPanel);
        container.GetComponent<ComponentBox>().ConfigureBox(component.name, sprite, color);
        return container;
    }

    public void SelectPack()
    {
        RewardCalculator.Main.ReceiveRewards(storagedPack.rewards);
        PackOfferManager.Main.RemovePack(storagedPack);
    }
}
