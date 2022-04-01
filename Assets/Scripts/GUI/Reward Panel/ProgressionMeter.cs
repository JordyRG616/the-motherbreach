using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using StringHandler;

public class ProgressionMeter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectMask2D mask;
    [SerializeField] private StatInfoBox infoBox;
    [SerializeField] private List<float> markers;
    private int markerPosition;
    private WaveManager waveManager;

    void Start()
    {
        waveManager = WaveManager.Main;
        ResetMarker();
    }

    public void ResetMarker()
    {
        markerPosition = 0;
        Vector4 pad = new Vector4(0, 0, markers[markerPosition], 0);
        mask.padding = pad;
    }

    public void AdvanceMarker()
    {
        markerPosition ++;
        Vector4 pad = new Vector4(0, 0, markers[markerPosition], 0);
        mask.padding = pad;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.GetComponent<RectTransform>().pivot = new Vector2 (0, 1);
        infoBox.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoBox.gameObject.SetActive(true);
        infoBox.GetComponent<RectTransform>().pivot = new Vector2 (1, 0);
        infoBox.GetComponent<StatInfoBox>().SetText(SetPreviewText());
    }

    private string SetPreviewText()
    {
        var container = "next wave:\n";
        var wave = waveManager.GetNextWave();
        container += StatColorHandler.StatPaint(wave.GetBreachCount() + " breaches") + "\nenemies detected:\n";
        foreach(string enemy in wave.enemiesInWave)
        {
            container += StatColorHandler.DamagePaint(enemy) + "\n";
        }

        container = "<size=125%>" + container;

        return container;
    }
}
