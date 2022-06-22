using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicShooter : MonoBehaviour
{
    [SerializeField] private ParticleSystem mirror;
    private ParticleSystem self;
    private GameManager gameManager;

    private void Start()
    {
        self = GetComponent<ParticleSystem>();
        gameManager = GameManager.Main;

        gameManager.OnGameStateChange += Copy;
    }

    private void Copy(object sender, GameStateEventArgs e)
    {
        self = mirror;
    }
}
