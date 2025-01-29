using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IinteractionInformationTaker 
{
    public void HasBeenHoldingSince(float time, bool holdingNow) { }
    public void InteractionStarted() { }
}
