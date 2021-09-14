using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget 
{
    public TargetType targetType{get; set;}
    public GameObject targetObject {get; set;}

    public void ReceiveObject(GameObject gameObject);
    public GameObject ReturnObject();
}
