using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void IniatiatePackChoice()
    {
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
        var qnt = (packs.Count >= 3)? 3 : packs.Count; 

        while(container.Count < qnt)
        {
            var rdm = Random.Range(0, packs.Count);
            if(!container.Contains(packs[rdm])) container.Add(packs[rdm]);
        }

        return container;
    }

    public void RemovePack(Pack pack)
    {
        packs.Remove(pack);

        foreach(PackBox box in packBoxes)
        {
            box.Disable();
        }

        blockPanel.SetActive(false);
        packOfferPanel.PlayReverse();
    }

}
