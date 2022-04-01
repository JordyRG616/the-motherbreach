using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactsPanel : MonoBehaviour
{
    public void CreateNewBox(Artifact artifact)
    {
        var container = Instantiate(ArtifactBox(artifact), Vector3.zero, Quaternion.identity, transform);
        container.GetComponent<ArtifactBox>().Initialize(artifact);
    }

    private GameObject ArtifactBox(Artifact artifact)
    {
        var container = new GameObject(artifact.name);
        container.AddComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        container.GetComponent<RectTransform>().localScale = new Vector2(1, -1);
        container.AddComponent<Image>().sprite = artifact.icon;
        container.AddComponent<ArtifactBox>();
        container.GetComponent<ArtifactBox>().Initialize(artifact);

        return container;
    }
}
