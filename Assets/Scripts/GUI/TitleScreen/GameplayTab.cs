using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayTab : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] private TMP_Dropdown movementDropdown;
    [SerializeField] private TMP_Dropdown rotationDropdown;

    void Start()
    {
        inputManager = InputManager.Main;
    }
    

    public void ShowTab()
    {
        gameObject.SetActive(true);
    }

    public void HideTab()
    {
        gameObject.SetActive(false);
    }

    public void HandleMovementDropdown(int value)
    {
        if(value == 0) inputManager.initializeWASDScheme();
        if(value == 1) inputManager.initializeArrowScheme();
        AudioManager.Main.RequestGUIFX("event:/UI/Title_Click2", out var index);
    }

    public void HandleRotationDropdown(int value)
    {
        if(value == 0) inputManager.initializeQEScheme();
        if(value == 1) inputManager.initializeMouseScheme();
        AudioManager.Main.RequestGUIFX("event:/UI/Title_Click2", out var index);
    }
}
