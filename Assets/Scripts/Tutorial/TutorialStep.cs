using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TutorialStep {
    public string stepHeader;
    public string stepComment;
    public List<SignalType> signalsNeededToComplete;
}
