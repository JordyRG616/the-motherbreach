using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectedShipPanel : MonoBehaviour
{
    [SerializeField] private List<ShipData> ships;
    [SerializeField] private List<Sprite> statRankSprites;
    public ShipData activeShip {get; private set;}
    
    [SerializeField] private Image shipImage;
    private Material imageMaterial;

    [SerializeField] private TextMeshProUGUI shipName;
    [SerializeField] private TextMeshProUGUI flavorText;
    [SerializeField] private TextMeshProUGUI abilityName;
    [SerializeField] private TextMeshProUGUI abilityDescription;

    [SerializeField] private Image healthRank;
    [SerializeField] private Image speedRank;
    [SerializeField] private Image handlingRank;

    [SerializeField] private GameObject lockedBox;
    [SerializeField] private List<Image> costsIndicators;
    [SerializeField] private Sprite powerCoreIcon;
    [SerializeField] private Sprite reinforcedCoreIcon;
    [SerializeField] private Sprite nobleCoreIcon;

    private LaunchButton launchButton;
    private MetaSaveFile save;
    private GameManager gameManager;

    private int activeIndex;

    void Start()
    {
        gameManager = GameManager.Main;

        imageMaterial = new Material(shipImage.material);
        shipImage.material = imageMaterial;

        launchButton = FindObjectOfType<LaunchButton>();
        ChangeSelection(0);

        save = DataManager.Main.metaProgressionSave;

        if(save == null || save.UnlockedShipsIndices == null) return;

        foreach(ShipData data in ships)
        {
            var id = data.ship.GetComponent<ShipManager>().index;
            if (save.UnlockedShipsIndices.Contains(id)) data.unlocked = true;
        }
    }

    public void ChangeSelection(int direction)
    {
        activeIndex += direction;

        if(activeIndex > ships.Count - 1) activeIndex = 0;
        if(activeIndex < 0) activeIndex = ships.Count - 1;

        activeShip = ships[activeIndex];


        shipImage.sprite = activeShip.shipSprite;

        shipName.text = activeShip.ship.name;
        flavorText.text = activeShip.flavorText;
        abilityName.text = activeShip.abilityName;
        abilityDescription.text = activeShip.abilityDescription;

        healthRank.sprite = statRankSprites[activeShip.healthRank];
        speedRank.sprite = statRankSprites[activeShip.speedRank];
        handlingRank.sprite = statRankSprites[activeShip.handlingRank];

        if(activeShip.unlocked) 
        {
            launchButton.ReceiveShip(activeShip.ship);
            lockedBox.SetActive(false);
            imageMaterial.SetInteger("_Unlocked", 1);
        } else
        {
            launchButton.ReceiveShip(null);
            lockedBox.SetActive(true);
            imageMaterial.SetInteger("_Unlocked", 0);
            SetCosts();
        }
    }

    private void SetCosts()
    {
        int costIndex = 0;

        costsIndicators.ForEach(x => x.enabled = false);

        for (int i = 0; i < activeShip.powerCoreCost; i++)
        {
            costsIndicators[costIndex].sprite = powerCoreIcon;
            costsIndicators[costIndex].enabled = true;
            costIndex++;
        }
        for (int i = 0; i < activeShip.reinforcedCoreCost; i++)
        {
            costsIndicators[costIndex].sprite = reinforcedCoreIcon;
            costsIndicators[costIndex].enabled = true;
            costIndex++;
        }
        for (int i = 0; i < activeShip.nobleCoreCost; i++)
        {
            costsIndicators[costIndex].sprite = nobleCoreIcon;
            costsIndicators[costIndex].enabled = true;
            costIndex++;
        }
    }
    
    public void UnlockShip()
    {
        if(!HasEnoughCore())
        {
            AudioManager.Main.PlayInvalidSelection("Not enough cores");
        } else
        {
            gameManager.UpdateCoreInventory(-activeShip.powerCoreCost, -activeShip.reinforcedCoreCost, -activeShip.nobleCoreCost);
            gameManager.UnlockShip(activeShip.ship.GetComponent<ShipManager>().index);
            FindObjectOfType<CoreInventory>().SetCoresValues();
            activeShip.unlocked = true;
            ChangeSelection(0);
        }
    }

    private bool HasEnoughCore()
    {
        if(save == null || save.UnlockedShipsIndices == null) return false;

        if(activeShip.powerCoreCost > gameManager.powerCoreAmount) return false;
        if(activeShip.reinforcedCoreCost > gameManager.reinforcedCoreAmount) return false;
        if(activeShip.nobleCoreCost > gameManager.nobleCoreAmount) return false;
        return true;
    }
}

[System.Serializable]
public class ShipData
{
    public GameObject ship;
    public Sprite shipSprite;
    [TextArea] public string flavorText;
    public bool unlocked;

    [Header("Cost")]
    public int powerCoreCost;
    public int reinforcedCoreCost;
    public int nobleCoreCost;

    [Header("Stats")]
    [Range(0, 4)] public int healthRank;
    [Range(0, 4)] public int speedRank;
    [Range(0, 4)] public int handlingRank;

    [Header("Ability")]
    public string abilityName;
    [TextArea] public string abilityDescription;
}
