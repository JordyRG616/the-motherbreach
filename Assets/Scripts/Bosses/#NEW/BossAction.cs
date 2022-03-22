using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAction : MonoBehaviour
{
    [Header("Offensive Action Parameters")]
    [SerializeField] protected List<ActionEffect> actionWeaponry;
    [Header("Movement Action Parameters")]
    public bool hasMove;
    [SerializeField] protected float speed;
    public float actionDuration;

    protected BossController controller;
    protected Rigidbody2D body;
    protected Transform ship;

    void Start()
    {
        controller = GetComponent<BossController>();
        body = GetComponent<Rigidbody2D>();
        ship = ShipManager.Main.transform;
        actionWeaponry.ForEach(x => x.Initiate());
        actionWeaponry.ForEach(x => x.ReceiveTarget(ship.gameObject));
    }

    public abstract void StartAction();
    public abstract void Action();
    public abstract void EndAction();

    public abstract void DoActionMove();
    
    protected void LookAt(Vector2 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}

[System.Serializable]
public struct ActionCombo
{
    [Range(0,1)] public float chance;
    public List<BossAction> actions;
    public float cooldown;
}
