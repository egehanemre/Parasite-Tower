using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private WaveManager waveManager;
    private UpgradeManager upgradeManager;
    [SerializeField] private int cost = 1;
    [SerializeField] private int cooldown = 60;
    [SerializeField] private GameObject detonateEffect;
    [SerializeField] private Transform detonationLocation;
    [SerializeField] private AudioClip audio;
    private float secondsRemaining = 0;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    private void FixedUpdate() {
        if(secondsRemaining > 0) secondsRemaining -= Time.fixedDeltaTime;
    }

    public void Detonate() {
        if(!waveManager || upgradeManager.Money < cost || secondsRemaining > 0) return;
        upgradeManager.Money -= cost;
        upgradeManager.UpdateMoneyText();
        waveManager.ClearEnemies();
        Instantiate(detonateEffect, detonationLocation.position, detonationLocation.rotation);
        secondsRemaining = cooldown;
        AudioSource.PlayClipAtPoint(audio, transform.position);
        TutorialSignalManager.PushSignal(SignalType.Bomb, false);
    }
}
