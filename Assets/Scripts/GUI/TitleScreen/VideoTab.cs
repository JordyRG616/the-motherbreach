using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoTab : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Toggle fullscreenToggle;
    private Vector2Int currentResolution;
    private bool fullscreen;

    void Start()
    {
        settings.resolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
        currentResolution = settings.resolution;
        fullscreen = settings.fullscreen;

        RegisterCurrentResolution();
        fullscreenToggle.isOn = fullscreen;

        Screen.SetResolution(currentResolution.x, currentResolution.y, fullscreen);
    }

    public void ShowTab()
    {
        gameObject.SetActive(true);
    }

    public void HideTab()
    {
        gameObject.SetActive(false);
    }

    public void SetResolution(int value)
    {
        switch(value)
        {
            case 0:
                currentResolution = new Vector2Int(1920, 1080);
            break;
            case 1:
                currentResolution = new Vector2Int(1366, 768);
            break;
            case 2:
                currentResolution = new Vector2Int(1280, 720);
            break;
        }

        settings.resolution = currentResolution;
        Screen.SetResolution(currentResolution.x, currentResolution.y, fullscreen);
    }

    private void RegisterCurrentResolution()
    {
        switch(currentResolution.x)
        {
            case 1920:
                dropdown.value = 0;
            break;
            case 1366:
                dropdown.value = 1;
            break;
            case 1280:
                dropdown.value = 2;
            break;
        }
    }

    public void SetFullscreen(bool toggle)
    {
        fullscreen = toggle;
        settings.fullscreen = fullscreen;
        Screen.SetResolution(currentResolution.x, currentResolution.y, fullscreen);
    }
}
