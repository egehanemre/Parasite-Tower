using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TutorialSignal {
    public SignalType signalType;
}

public enum SignalType {
    Harvest,
    Feed,
    Fix,
    Extinguish,
    Task,
    Bomb,
    Upgrade
}
