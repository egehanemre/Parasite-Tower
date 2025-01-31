using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<TutorialStep> steps = new List<TutorialStep>();
    [SerializeField] private int currentStepIndex = 0;
    private bool finished = false;

    private void Start()
    {
        if(steps == null || steps.Count == 0) return;
        TutorialSignalManager.instance.onSignalTaken.AddListener(TickCurrentStep);
        TutorialRenderer.instance.Possess(steps[currentStepIndex]);
    }

    public void TickCurrentStep(TutorialSignal signal) {
        if(finished) return;
        
        TutorialStep currentStep = steps[currentStepIndex];
        for (int i = 0; i < currentStep.signalsNeededToComplete.Count; i++)
        {
            if (currentStep.signalsNeededToComplete[i] == signal.signalType) {
                currentStep.signalsNeededToComplete.RemoveAt(i);
                break;
            }
        }

        if (currentStep.signalsNeededToComplete.Count == 0) {
            GoNextStep();
        }
        else {
            steps[currentStepIndex] = currentStep;
        }
    }

    public void GoNextStep() {
        currentStepIndex++;

        if (currentStepIndex >= steps.Count) {
            Destroy(TutorialRenderer.instance.gameObject);
            finished = true;
            return;
        }
        
        TutorialRenderer.instance.Possess(steps[currentStepIndex]);
        foreach (var previousSignal in TutorialSignalManager.instance.previousSignals) {
            TickCurrentStep(previousSignal);
        }
    }
    
}
