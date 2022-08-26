using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomRandom;

public class PackOfferManager : MonoBehaviour
{
    #region Singleton
    private static PackOfferManager _instance;
    public static PackOfferManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<PackOfferManager>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<PackOfferManager>();
                }
            }
            return _instance;
        }
    }
    #endregion


    [SerializeField] private UIAnimations packOfferPanel;
    [SerializeField] private List<PackBox> packBoxes;
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private List<Pack> packs;
    private List<Pack> unlockedPacks = new List<Pack>();
    private List<int> unlockedIndexes = new List<int>();
    private bool tutorialTaken;

    void Start()
    {
        CheckLockedPacks();
    }

    private void CheckLockedPacks()
    {
        foreach(Pack pack in packs)
        {
            if(pack.requiredIndexes.Count == 0) UnlockPack(pack);
            else
            {
                CheckPackIndexes(pack);
            }
        }
    }

    private void CheckPackIndexes(Pack pack)
    {
        foreach (int index in pack.requiredIndexes)
        {
            if (!unlockedIndexes.Contains(index)) return;
        }

        UnlockPack(pack);
    }

    private void UnlockPack(Pack pack)
    {
        if(unlockedPacks.Contains(pack)) return;
        unlockedPacks.Add(pack);
    }

    public void IniatiatePackChoice()
    {
        if(!tutorialTaken)
        {
            FindObjectOfType<TutorialManager>().TriggerTechTutorial();
            tutorialTaken = true;
        }
        var rewardPacks = SelectPacks();

        for(int i = 0; i < packBoxes.Count; i++)
        {
            if(i >= rewardPacks.Count) break;            
            
            packBoxes[i].ConfigureBox(rewardPacks[i]);
        }

        blockPanel.SetActive(true);
        packOfferPanel.Play();
    }

    private List<Pack> SelectPacks()
    {
        var container = new List<Pack>();
        var qnt = (unlockedPacks.Count >= 3)? 3 : unlockedPacks.Count; 

        while(container.Count < qnt)
        {
            var rdm = RandomManager.GetRandomInteger(0, unlockedPacks.Count);
            if(!container.Contains(unlockedPacks[rdm])) container.Add(unlockedPacks[rdm]);
        }

        return container;
    }

    public void RemovePack(Pack pack)
    {
        unlockedPacks.Remove(pack);
        packs.Remove(pack);
        unlockedIndexes.Add(pack.index);
        CheckLockedPacks();

        foreach(PackBox box in packBoxes)
        {
            box.Disable();
        }

        blockPanel.SetActive(false);
        packOfferPanel.PlayReverse();
    }

}
