using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Wave/Nebula", fileName ="New Nebula")]
public class Nebula: ScriptableObject
{
    public Material firstNebulaMaterial;
    public Material secondNebulaMaterial;
    public Material thirdNebulaMaterial;
    public Color backgroundColor;

    public RenderTextureParameters renderParamenters;
}
