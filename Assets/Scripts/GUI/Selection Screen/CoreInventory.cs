using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoreInventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI powerCoreValue;
    [SerializeField] private TextMeshProUGUI reinforcedCoreValue;
    [SerializeField] private TextMeshProUGUI nobleCoreValue;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Main;

        SetCoresValues();
    }

    public void SetCoresValues()
    {
        powerCoreValue.text = gameManager.powerCoreAmount.ToString();
        reinforcedCoreValue.text = gameManager.reinforcedCoreAmount.ToString();
        nobleCoreValue.text = gameManager.nobleCoreAmount.ToString();
    }
}
