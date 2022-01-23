using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayTab : MonoBehaviour
{
    private InputManager inputManager;

    void Start()
    {
        inputManager = InputManager.Main;
    }
    
    public void PlayClickSFX()
    {
        AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click1");
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
        AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click2");
    }

    public void HandleRotationDropdown(int value)
    {
        if(value == 0) inputManager.initializeQEScheme();
        if(value == 1) inputManager.initializeMouseScheme();
        AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click2");
    }
}
