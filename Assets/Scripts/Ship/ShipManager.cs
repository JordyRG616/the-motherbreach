using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    #region Singleton
    private static ShipManager _instance;
    public static ShipManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ShipManager>();
            }
            return _instance;
        }
    }
    #endregion

    private GameManager gameManager;
    private List<TurretManager> turrets = new List<TurretManager>();

    void Awake()
    {
        // gameManager = GameManager.Main;
        // gameManager.OnGameStateChange += HandleRotationReset;
    }

    private void HandleRotationReset(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnReward)
        {
            StartCoroutine(ResetRotation());
        }
        if(e.newState == GameState.OnWave)
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator ResetRotation()
    {
        float step = 0;

        while(step < 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, step);
            step += .1f;
            yield return new WaitForSecondsRealtime(.01f);
        }
    }

    public List<BaseEffectTemplate> GetBases()
    {
        List<BaseEffectTemplate> container = new List<BaseEffectTemplate>();

        foreach(TurretManager turret in turrets)
        {
            container.Add(turret.BaseEffect);
        }

        return container;
    }

    public List<ActionController> GetWeapons()
    {
        List<ActionController> container = new List<ActionController>();

        foreach(TurretManager turret in turrets)
        {
            container.Add(turret.ActionController);
        }

        return container;
    }

}
