using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointer : MonoBehaviour
{
    private Camera mainCamera;
    private (float min, float max) verticalBoundaries;
    private (float min, float max) horizontalBoundaries;
    [SerializeField] private Transform target;
    private Transform ship;
    private Transform pointer;
    private Transform icon;
    private Transform anchor;


    void Start()
    {
        mainCamera = Camera.main;

        verticalBoundaries.max = mainCamera.orthographicSize;
        verticalBoundaries.min = -verticalBoundaries.max;

        horizontalBoundaries.max = verticalBoundaries.max * (16f / 9f);
        horizontalBoundaries.min = -horizontalBoundaries.max;

        ship = FindObjectOfType<ShipManager>().transform;
        pointer = transform.Find("Pointer");
        icon = transform.Find("Icon");
        anchor = pointer.Find("Anchor");
    }

    public void ReceiveTarget(Transform target)
    {
        this.target = target;
    }

    private Vector3 CalculatePosition(Vector3 position)
    {
        if(position.y > verticalBoundaries.max) position.y = verticalBoundaries.max;
        if(position.y < verticalBoundaries.min) position.y = verticalBoundaries.min;
        if(position.x > horizontalBoundaries.max) position.x = horizontalBoundaries.max;
        if(position.x < horizontalBoundaries.min) position.x = horizontalBoundaries.min;
        return position;
    }

    private Quaternion CalculateRotation(Vector3 position)
    {
        // if(transform.position.y + ship.transform.position.y > 0) return Quaternion.Euler(0, 0, 0);
        // if(transform.position.y + ship.transform.position.y < 0) return Quaternion.Euler(0, 0, 180);
        // if(transform.position.x + ship.transform.position.x > 0) return Quaternion.Euler(0, 0, -90);
        // if(transform.position.x + ship.transform.position.x < 0) return Quaternion.Euler(0, 0, 90);
        // return Quaternion.identity;

        var angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, angle - 90f);
    }

    void Update()
    {
        if(target != null)
        {
            var position =  target.position - ship.transform.position;
            HandleView(TargetInView(position));
            transform.position = ship.transform.position + CalculatePosition(position);
            pointer.rotation = CalculateRotation(position);
            icon.position = anchor.position;
        } else
        {
            Destroy(gameObject);
        }
    }

    private bool TargetInView(Vector3 position)
    {
        if(position.y > verticalBoundaries.max || position.y < verticalBoundaries.min) return false;
        if(position.x > horizontalBoundaries.max || position.x < horizontalBoundaries.min) return false;
        return true;
    }

    private void HandleView(bool inView)
    {
        pointer.gameObject.SetActive(!inView);
        icon.gameObject.SetActive(!inView);
    }

}
