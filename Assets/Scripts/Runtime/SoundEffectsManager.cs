using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;
    public List<SoundEffectInstance> soundEffectInstances = new List<SoundEffectInstance>();

    private void Awake() {
        instance = this;
    }

    public void SetStateOfEffect(string soundId, bool active) {
        foreach (var soundEffect in soundEffectInstances) {
            if (soundEffect.id != soundId) continue;
                
            soundEffect.soundSource.mute = !active;
            foreach (var subSource in soundEffect.subSources) {
                subSource.mute = !active;
            }
        }
    }
}

[Serializable]
public class SoundEffectInstance {
    public AudioSource soundSource;
    public List<AudioSource> subSources;
    public string id;
}
