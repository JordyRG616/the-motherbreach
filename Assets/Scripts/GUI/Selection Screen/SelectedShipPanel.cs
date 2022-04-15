using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectedShipPanel : MonoBehaviour
{
    [SerializeField] private List<ShipData> ships;
    [SerializeField] private List<Sprite> statRankSprites;
    
    [SerializeField] private Image shipImage;

    [SerializeField] private TextMeshProUGUI shipName;
    [SerializeField] private TextMeshProUGUI flavorText;
    [SerializeField] private TextMeshProUGUI abilityName;
    [SerializeField] private TextMeshProUGUI abilityDescription;

    [SerializeField] private Image healthRank;
    [SerializeField] private Image speedRank;
    [SerializeField] private Image handlingRank;

    private LaunchButton launchButton;

    private int activeIndex;

    void Start()
    {
        launchButton = FindObjectOfType<LaunchButton>();
        ChangeSelection(0);
    }

    public void ChangeSelection(int direction)
    {
        activeIndex += direction;

        if(activeIndex > ships.Count - 1) activeIndex = 0;
        if(activeIndex < 0) activeIndex = ships.Count - 1;

        var data = ships[activeIndex];

        launchButton.ReceiveShip(data.ship);

        shipImage.sprite = data.shipSprite;
        shipName.text = data.ship.name;
        flavorText.text = data.flavorText;
        abilityName.text = data.abilityName;
        abilityDescription.text = data.abilityDescription;

        healthRank.sprite = statRankSprites[data.healthRank];
        speedRank.sprite = statRankSprites[data.speedRank];
        handlingRank.sprite = statRankSprites[data.handlingRank];
    }
    
}

[System.Serializable]
public class ShipData
{
    public GameObject ship;
    public Sprite shipSprite;
    [TextArea] public string flavorText;

    [Range(0, 4)] public int healthRank;
    [Range(0, 4)] public int speedRank;
    [Range(0, 4)] public int handlingRank;

    public string abilityName;
    [TextArea] public string abilityDescription;
}
