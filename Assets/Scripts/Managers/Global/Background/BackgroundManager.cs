using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer firstNebula;
    [SerializeField] private SpriteRenderer secondNebula;
    [SerializeField] private SpriteRenderer thirdNebula;
    [SerializeField] private Material renderMaterial;

    public void ChangeBackground()
    {
        var nebula = NodeMapManager.Main.selectedNode.nodeWave.nebulaSetting;
        Initialize(nebula);
    }

    private void Initialize(Nebula nebula)
    {
        firstNebula.material = nebula.firstNebulaMaterial;
        secondNebula.material = nebula.secondNebulaMaterial;
        thirdNebula.material = nebula.thirdNebulaMaterial;
        SetCameraBackground(nebula.backgroundColor);
        SetRenderParameters(nebula.renderParamenters);
    }

    private void SetCameraBackground(Color color)
    {
        cam.backgroundColor = color;
    }

    private void SetRenderParameters(RenderTextureParameters parameters)
    {
        renderMaterial.SetFloat("_Temp", parameters.temperature);
        renderMaterial.SetFloat("_Tint", parameters.tint);
        renderMaterial.SetFloat("_Contrast", parameters.contrast);
    }
}

[Serializable]
public struct RenderTextureParameters
{
    [Range(-1, 1)] public float temperature;
    [Range(-1, 1)] public float tint;
    [Range(0, 1)] public float contrast;
}
