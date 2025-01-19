using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shadeSlider;
    [SerializeField] private float shadeValueCatchingSpeed = 1;
    private float shadeValue;
    private float baseShadeValue;
    private float targetValue;
    private float shadeProgress;
    private float healthProgress;

    public void UpdateValue(float newValue) {
        targetValue = newValue;
        baseShadeValue = shadeValue;
        shadeProgress = 0;
        healthSlider.value = newValue;
    }

    public void Update() {
        if(shadeProgress > 1) return;
        
        shadeProgress += Time.deltaTime * shadeValueCatchingSpeed;
        shadeValue = Mathf.Lerp(baseShadeValue, targetValue, shadeProgress);
        shadeSlider.value = shadeValue;
    }
}
