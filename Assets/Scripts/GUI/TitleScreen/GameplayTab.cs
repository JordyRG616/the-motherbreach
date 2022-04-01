using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using StringHandler;

public class GameplayTab : MonoBehaviour
{
    private InputManager inputManager;
    private KeyCode[] codes;
    private KeyCode newKey;
    private WaitForEndOfFrame wait = new WaitForEndOfFrame();
    [SerializeField] private Toggle mouseToggle;
    [SerializeField] private Settings settings;
    [Header("Rotation texts")]
    [SerializeField] private TextMeshProUGUI rightRotationText;
    [SerializeField] private TextMeshProUGUI leftRotationText;
    [Header("Movement texts")]
    [SerializeField] private TextMeshProUGUI upKey;
    [SerializeField] private TextMeshProUGUI leftKey;
    [SerializeField] private TextMeshProUGUI downKey;
    [SerializeField] private TextMeshProUGUI rightKey;
    private bool useMouse = true;


    void Start()
    {
        inputManager = InputManager.Main;
        codes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        SetTexts();
        EnableMovementKeyBinding();

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

    public void ToggleMouseControl(bool useMouse)
    {
        this.useMouse = useMouse;
        settings.useMouse = useMouse;
        inputManager.SwitchMovementScheme(useMouse);
        EnableMovementKeyBinding();
    }

    private void EnableMovementKeyBinding()
    {
        if(useMouse)
        {
            upKey.color = Color.red;
            leftKey.color = Color.red;
            downKey.color = Color.red;
            rightKey.color = Color.red;
        } else
        {
            upKey.color = Color.white;
            leftKey.color = Color.white;
            downKey.color = Color.white;
            rightKey.color = Color.white;
        }
    }

    public void BindRotation(int direction)
    {
        AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click1");
        StartCoroutine(WaitForRotationKey(direction));
    }

    public void BindMovement(int direction)
    {
        if(useMouse) return;
        AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click1");
        StartCoroutine(WaitForMovementKey(direction));
    }

    private void SetMovementKey(int direction)
    {
        switch(direction)
        {
            case 0:
                upKey.text = newKey.ToSplittedString();
                AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click2");
                inputManager.upKey = newKey;
                settings.moveUp = newKey;
            break;
            case 1:
                leftKey.text = newKey.ToSplittedString(); 
                AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click2");
                inputManager.leftKey = newKey;
                settings.moveLeft = newKey;
            break;
            case 2:
                downKey.text = newKey.ToSplittedString(); 
                AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click2");
                inputManager.downKey = newKey;
                settings.moveDown = newKey;
            break;
            case 3:
                rightKey.text = newKey.ToSplittedString(); 
                AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click2");
                inputManager.rightKey = newKey;
                settings.moveRight = newKey;
            break;
        }
    }

    private void SetTexts()
    {
        upKey.text = settings.moveUp.ToSplittedString();
        leftKey.text = settings.moveLeft.ToSplittedString(); 
        downKey.text = settings.moveDown.ToSplittedString(); 
        rightKey.text = settings.moveRight.ToSplittedString(); 

        leftRotationText.text = settings.rotateLeft.ToSplittedString();
        rightRotationText.text = settings.rotateRight.ToSplittedString();

        useMouse = settings.useMouse;
        mouseToggle.isOn = useMouse;
    }

    private void SetRightRotation()
    {
        rightRotationText.text = newKey.ToString();
        inputManager.rotateLeft = newKey;
        settings.rotateRight = newKey;
        AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click2");
    }

    private void SetLeftRotation()
    {
        leftRotationText.text = newKey.ToString();
        inputManager.rotateRight = newKey;
        settings.rotateLeft = newKey;
        AudioManager.Main.RequestGUIFX("event:/UI/Title/Title_Click2");
    }

    private IEnumerator WaitForRotationKey(int direction)
    {
        while(true)
        {
            foreach(KeyCode code in codes)
            {
                if(Input.GetKey(code))
                {
                    newKey = code;
                    if(direction == 0) SetRightRotation();
                    else SetLeftRotation();
                    StopAllCoroutines();
                }
            }

            yield return wait;
        }
    }

    private IEnumerator WaitForMovementKey(int direction)
    {
        while(true)
        {
            foreach(KeyCode code in codes)
            {
                if(Input.GetKey(code))
                {
                    newKey = code;
                    SetMovementKey(direction);
                    StopAllCoroutines();
                }
            }

            yield return wait;
        }
    }
}
