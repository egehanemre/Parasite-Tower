using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSignalManager : MonoBehaviour
{
    public List<TutorialSignal> previousSignals = new List<TutorialSignal>();
    public static TutorialSignalManager instance;
    public UnityEvent<TutorialSignal> onSignalTaken;

    private void Awake() {
        instance = this;
    }

    public static void PushSignal(TutorialSignal signal, bool remember) {
        instance.onSignalTaken?.Invoke(signal);
        if(remember) instance.previousSignals.Add(signal);
    }
    
    public static void PushSignal(SignalType signalType, bool remember)
    {
        TutorialSignal signal = new TutorialSignal() {
            signalType = signalType
        };
        instance.onSignalTaken?.Invoke(signal);
        if(remember) instance.previousSignals.Add(signal);
    }
}
