using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Foundation : MonoBehaviour, ISavable
{
    [SerializeField] protected int _id;
    public int Id { get => _id; }
    [SerializeField] protected int _cost;
    public int Cost { get => _cost; }
    public List<TurretStat> exposedStats { get; protected set; } = new List<TurretStat>();
    [SerializeField] protected List<Program> _programs;
    public List<Program> Programs { get => _programs; }
    protected Weapon linkedWeapon;


    public virtual void Initiate(Weapon weapon)
    {
        linkedWeapon = weapon;
        exposedStats.ForEach(x => linkedWeapon.ExposeDormentStat(x));

        var cache = Programs.ToList();
        Programs.Clear();

        weapon.InitialPrograms.ForEach(x => ReceiveProgram(x));
        weapon.InitialPrograms.Clear();
        cache.ForEach(x => ReceiveProgram(x));
    }

    private void Awake()
    {
        exposedStats = GetComponents<TurretStat>().ToList();
    }

    public void ReceiveProgram(Program program)
    {
        var type = program.GetType();
        var programInstance = (Program)ScriptableObject.CreateInstance(type);
        programInstance.Initiate();
        Programs.Add(program);
    }

    public virtual string Description()
    {
        string stats = "";

        foreach(TurretStat stat in GetComponents<TurretStat>())
        {
            stats += " " + stat.publicName + ",";
        }

        string program = "";

        if (Programs[0] != null) program += Programs[0].name + " (" + Programs[0].Description() + ")";

        return "Exposes" + stats + " and enables " + program + ".";
    }

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();
        var slotId = GetComponentInParent<TurretManager>().slotId;
        int index = 0;

        foreach(Program program in Programs)
        {
            var key = slotId + "Program" + index;
            index++;
            var value = BitConverter.GetBytes(program.Id);
            container.Add(key, value);
        }

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        var slotId = GetComponentInParent<TurretManager>().slotId;

        Programs.Clear();

        for (int i = 0; i < 5; i++)
        {
            var key = slotId + "Program" + i;
            if (!saveFile.ContainsSavedContent(key)) break;

            var value = BitConverter.ToInt32(saveFile.GetValue(key));
            var program = TurretConstructor.Main.GetProgramById(value);
            ReceiveProgram(program);
        }
    }
}
