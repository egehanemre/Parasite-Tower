using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldProgressBar : MonoBehaviour
{
    public static HoldProgressBar actionProgressBar;
    private Image image;

    public void Render(bool activated, float progress) {
        gameObject.SetActive(activated);
        image.fillAmount = progress;
    }

    private void Awake() {
        image = GetComponent<Image>();
        actionProgressBar = this;
    }
}
