using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningManager : MonoBehaviour {
    [SerializeField] private GameObject onFireWarning;
    [SerializeField] private GameObject onElectricWarning;
    [SerializeField] private float warningUpdateTime = 2;
    private float timePassed = 0;
    
    public void Possess(bool hasFire, bool hasElectric) {
        onElectricWarning.SetActive(hasElectric);
        onFireWarning.SetActive(hasFire);
    }

    private void Update() {
        timePassed += Time.deltaTime;
        if (timePassed > warningUpdateTime) {
            timePassed = 0;
            List<EventData> eventDatas = EventManager.eventManager.GetAllActiveEventDatas();
            bool hasFire = false;
            bool hasElectric = false;
            foreach (var eventData in eventDatas) {
                if (eventData.eventType == EventType.Fire) hasFire = true;
                if (eventData.eventType == EventType.Electric) hasElectric = true;
            }
            Possess(hasFire, hasElectric);
        }
    }
}
