using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Foundation : MonoBehaviour
{
    [SerializeField] protected int _id;
    public int Id { get => _id; }
    [SerializeField] protected int _cost;
    public int Cost { get => _cost; }
    public List<TurretStat> exposedStats { get; protected set; } = new List<TurretStat>();
    public List<Program> programs { get; private set; } = new List<Program>();


    public virtual void Initiate()
    {

    }

    private void Awake()
    {
        exposedStats = GetComponents<TurretStat>().ToList();
    }
}
