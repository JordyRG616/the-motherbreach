using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorDeployable : MonoBehaviour
{

    public void Die()
    {
        Destroy(gameObject);
    }

}
