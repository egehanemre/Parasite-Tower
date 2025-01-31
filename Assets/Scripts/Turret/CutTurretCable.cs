using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTurretCable : MonoBehaviour, IinteractionInformationTaker
{
    [SerializeField] private AudioSource fixingSound;
    [SerializeField] private Turret linkedTurret;
    [SerializeField] private float playCutSoundDelay;
    [SerializeField] private AudioClip cutSound;
    [SerializeField] private float volume = 0.03f;
    private float timePassed = 0;
    
    public void Fix() {
        linkedTurret.RecoverEnergy();
        gameObject.SetActive(false);
        TutorialSignalManager.PushSignal(SignalType.Task, false);
        TutorialSignalManager.PushSignal(SignalType.Fix, false);
    }
    
    
    public void HasBeenHoldingSince(float time, bool holdingNow) {
        fixingSound.mute = !holdingNow;
    }

    private void Update() {
        if (gameObject.activeSelf) {
            timePassed += Time.deltaTime;
            if (timePassed > playCutSoundDelay) {
                AudioSource.PlayClipAtPoint(cutSound, transform.position, volume);
                timePassed = 0;
            }
        }
    }
}