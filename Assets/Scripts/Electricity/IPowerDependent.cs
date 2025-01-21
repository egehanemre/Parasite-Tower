using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerDependent
{
    public void SetPowerState(bool state);
    public bool GetPowerState();
}
