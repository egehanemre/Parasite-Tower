using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;
    public static bool isTowerMode => instance.currentMode == "tower";
    public static bool isGunnerMode => instance.currentMode == "gunner";
    public List<SoundEffectInstance> soundEffectInstances = new List<SoundEffectInstance>();
    public string currentMode;

    private void Awake() {
        instance = this;
    }

    public void SetEffectPackActivation(string soundType, bool activation) {
        foreach (var soundEffect in soundEffectInstances) {
            if (soundEffect.id != soundType) continue;
                
            soundEffect.soundSource.mute = !activation;
            foreach (var subSource in soundEffect.subSources) {
                subSource.mute = !activation;
            }
        }

        if(activation)currentMode = soundType;
    }
}

[Serializable]
public class SoundEffectInstance {
    public AudioSource soundSource;
    public List<AudioSource> subSources;
    public string id;
}
