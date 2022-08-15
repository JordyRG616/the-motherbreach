using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private List<Nebula> nebulas;
    [SerializeField] private Camera cam;
    [SerializeField] private List<Color> cameraBGColors;

    private void Start()
    {
        nebulas.ForEach(x => x.SetRandomMaterial());
        SetCameraBackground();
    }

    private void SetCameraBackground()
    {
        var rdm = Random.Range(0, cameraBGColors.Count);

        cam.backgroundColor = cameraBGColors[rdm];
    }
}
