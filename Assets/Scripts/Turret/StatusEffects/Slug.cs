using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : MonoBehaviour
{
    private MovableEntity enemy;

    void Start()
    {
        enemy = GetComponent<MovableEntity>();
        StartCoroutine(SlowDown());
    }
    
    private IEnumerator SlowDown()
    {
        enemy.AddDrag(0.33f);

        yield return new WaitForSecondsRealtime(2f);

        enemy.RemoveDrag();

        Terminate();
    }

    private void Terminate()
    {
        Destroy(this);
    }
}
