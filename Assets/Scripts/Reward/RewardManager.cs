using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    private TurretConstructor turretConstructor;
    private RewardCalculator calculator;

    void Start()
    {
        Initiate();
    }

    public void Initiate()
    {
        turretConstructor = TurretConstructor.Main;
        calculator = RewardCalculator.Main;
    }

    private void GenerateReward()
    {
        
    }
}
