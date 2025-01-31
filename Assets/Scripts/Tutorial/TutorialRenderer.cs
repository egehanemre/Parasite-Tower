using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialRenderer : MonoBehaviour
{
    public static TutorialRenderer instance;
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI textField;

    private void Awake() {
        instance = this;
    }

    public void Possess(TutorialStep step) {
        headerField.text = step.stepHeader;
        textField.text = step.stepComment;
    }
}
