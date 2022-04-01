using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomAnimation : UIAnimations
{
    public override bool Done { get; protected set; }

    private Camera cam;
    [SerializeField] private float targetOrtographicSize;
    [SerializeField] private Vector2 slideAmount;
    private float ogSize;

    protected override void Awake()
    {
        cam = GetComponent<Camera>();

        base.Awake();
    }

    public override IEnumerator Forward()
    {
        GetComponent<CameraFollowComponent>().enabled = false;
        float step = 0;
        ogSize = cam.orthographicSize;
        var pos = cam.transform.position;

        while (step <= duration)
        {
            var size = Mathf.Lerp(ogSize, targetOrtographicSize, step / duration);
            cam.orthographicSize = size;

            var slide = Vector3.Lerp(pos, (Vector2)pos + slideAmount, step / duration);
            slide.z = -10;
            cam.transform.position = slide;

            step += animationSpeed;
            yield return waitTime;
        }
    }

    public override IEnumerator Reverse()
    {
        float step = 0;
        var pos = cam.transform.position;

        while (step <= duration)
        {
            var size = Mathf.Lerp(targetOrtographicSize, ogSize, step / duration);
            cam.orthographicSize = size;

            var slide = Vector3.Lerp(pos, (Vector2)pos - slideAmount, step / duration);
            slide.z = -10;
            cam.transform.position = slide;

            step += animationSpeed;
            yield return waitTime;
        }

        GetComponent<CameraFollowComponent>().enabled = true;
    }
}
