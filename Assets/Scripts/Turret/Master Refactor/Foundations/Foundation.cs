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
    [SerializeField] protected List<Trait> _programs;
    public List<Trait> Programs { get => _programs; }
    protected Weapon linkedWeapon;


    public virtual void Initiate(Weapon weapon)
    {
        linkedWeapon = weapon;
        exposedStats.ForEach(x => linkedWeapon.ExposeDormentStat(x));

        var cache = Programs.ToList();
        Programs.Clear();

        weapon.InitialPrograms.ForEach(x => ReceiveTrait(x));
        weapon.InitialPrograms.Clear();
        cache.ForEach(x => ReceiveTrait(x));
    }

    private void Awake()
    {
        exposedStats = GetComponents<TurretStat>().ToList();
    }

    public void ReceiveTrait(Trait program)
    {
        var programInstance = program.ReturnTraitInstance();
        programInstance.Initiate(linkedWeapon);
        Programs.Add(programInstance);
    }

    public virtual string Description()
    {
        string stats = "";

        foreach(TurretStat stat in GetComponents<TurretStat>())
        {
            stats += " " + stat.publicName + ",";
        }

        string program = "";

        if (Programs[0] != null) program += Programs[0].name + " - <i> " + Programs[0].Description() + ".</i>";

        return "Exposes" + stats + " and enables " + program + ".";
    }

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();
        var slotId = GetComponentInParent<TurretManager>().slotId;
        int index = 0;

        foreach(Trait program in Programs)
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
            ReceiveTrait(program);
        }
    }
}
