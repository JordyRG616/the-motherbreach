using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    private InputManager inputManager;

    void Awake()
    {
        inputManager = InputManager.Main;

        inputManager.OnSelectionClear += Disable;
    }

    private void Disable(object sender, EventArgs e)
    {
        inputManager.OnSelectionClear -= Disable;
        gameObject.SetActive(false);
    }
}
