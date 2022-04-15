using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PilotInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pilotName;
    [SerializeField] private TextMeshProUGUI flavorText;
    [SerializeField] private TextMeshProUGUI abilityDescription;

    public void ReceivePilotInfo(PilotBox box)
    {
        pilotName.text = box.pilot.name;
        flavorText.text = box.pilot.flavorText;
        abilityDescription.text = box.pilot.AbilityDescription();
    }
}
