using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowComponent : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private float followDistance;
    [SerializeField] private float followSpeed;
    private bool following;

    void OnEnable()
    {
        Vector3 direction = objectToFollow.position - transform.position + new Vector3(0, 0, transform.position.z);
        StartCoroutine(Follow(direction));
    }

    private IEnumerator Follow(Vector3 direction)
    {
        while(!Mathf.Approximately(direction.magnitude, 0))
        {
            transform.position += direction * followSpeed;
            yield return new WaitForSecondsRealtime(.01f);
            direction = objectToFollow.position - transform.position + new Vector3(0, 0, transform.position.z);
        }

        following = false;

        StopCoroutine("Follow");
    }

    void Update()
    {
        Vector3 direction = objectToFollow.position - transform.position + new Vector3(0, 0, transform.position.z);
        if(direction.magnitude >= followDistance && !following)
        {
            following = true;
            StartCoroutine(Follow(direction));
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
