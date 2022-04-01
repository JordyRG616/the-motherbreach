using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using StringHandler;

public class ArtifactBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Artifact artifact;
    private RectTransform infoBox;

    void Awake()
    {
        infoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoBox.gameObject.SetActive(true);
        infoBox.GetComponent<StatInfoBox>().SetText(StatColorHandler.StatPaint(artifact.name) + "\n\n" + artifact.Description());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.gameObject.SetActive(false);
    }

    public void Initialize(Artifact artifact)
    {
        this.artifact = artifact;
    }

    void Update()
    {
        if(infoBox.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition + new Vector3(2, -2) - new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0);
            infoBox.anchoredPosition = mousePos;
        }
    }
}
