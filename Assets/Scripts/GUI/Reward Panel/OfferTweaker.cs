using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OfferTweaker : MonoBehaviour
{
    [SerializeField] private GameObject componentBox;
    [SerializeField] private RectTransform weaponsPanel, basesPanel;
    [SerializeField] private Color weaponColor, baseColor;
    [SerializeField] private TextMeshProUGUI removalTxt;
    [SerializeField] [FMODUnity.EventRef] private string selectionSFX;
   
    public int removalPoints;
    private RewardCalculator rewardCalculator;
    private List <GameObject> activeBoxes = new List<GameObject>();
    private List<GameObject> selectedComponents = new List<GameObject>();
    private UIAnimations anim;

    void Start()
    {
        rewardCalculator = RewardCalculator.Main;
        anim = GetComponent<UIAnimations>();
    }

    public void Open()
    {
        anim.Play();
        GenerateComponents();
        weaponsPanel.GetComponent<ScrollablePanel>().Initiate();
        basesPanel.GetComponent<ScrollablePanel>().Initiate();
    }

    private void GenerateComponents()
    {
        DestroyBoxes();

        foreach (GameObject weapon in rewardCalculator.weapons)
        {
            activeBoxes.Add(NewWeaponBox(weapon));
        }

        foreach (GameObject _base in rewardCalculator.bases)
        {
            activeBoxes.Add(NewBaseBox(_base));
        }
    }

    private void DestroyBoxes()
    {
        foreach (GameObject box in activeBoxes)
        {
            Destroy(box);
        }
        activeBoxes.Clear();
    }

    private GameObject NewWeaponBox(GameObject component)
    {
        Sprite sprite = component.GetComponent<SpriteRenderer>().sprite;
        var container = Instantiate(componentBox, Vector3.zero, Quaternion.identity, weaponsPanel);
        var box = container.GetComponent<ComponentBox>();
        box.ConfigureBox(component.name, sprite, weaponColor);
        box.ReceiveComponentObject(component);
        box.OnClick += HandleComponentClick;
        return container;
    }


    private GameObject NewBaseBox(GameObject component)
    {
        Sprite sprite = component.GetComponent<SpriteRenderer>().sprite;
        var container = Instantiate(componentBox, Vector3.zero, Quaternion.identity, basesPanel);
        var box = container.GetComponent<ComponentBox>();
        box.ConfigureBox(component.name, sprite, baseColor);
        box.ReceiveComponentObject(component);
        box.OnClick += HandleComponentClick;
        return container;
    }

    private void HandleComponentClick(GameObject component)
    {
        if (selectedComponents.Contains(component))
        {
            selectedComponents.Remove(component);
        }
        else
        {
            if (SelectionFull())
            {
                AudioManager.Main.PlayInvalidSelection();
                return;
            }
            selectedComponents.Add(component);
            AudioManager.Main.RequestGUIFX(selectionSFX);
        }


    }

    public bool SelectionFull()
    {
        return selectedComponents.Count == removalPoints;
    }

    public void Remove()
    {
        foreach(GameObject component in selectedComponents)
        {
            if(rewardCalculator.weapons.Contains(component)) rewardCalculator.weapons.Remove(component);
            if(rewardCalculator.bases.Contains(component)) rewardCalculator.bases.Remove(component);
            removalPoints --;
        }

        selectedComponents.Clear();
        GenerateComponents();
        weaponsPanel.GetComponent<ScrollablePanel>().Initiate();
        basesPanel.GetComponent<ScrollablePanel>().Initiate();
    }

    public void Close()
    {
        anim.PlayReverse();
        DestroyBoxes();
    }
    
    void Update()
    {
        removalTxt.text = removalPoints + " removal points";
    }
}
