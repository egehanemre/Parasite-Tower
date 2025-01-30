using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldButton : MonoBehaviour
{

    [SerializeField] private ShieldProtectionSide attachedSide;
    [SerializeField] private GameObject onActivated;
    [SerializeField] private GameObject onDeactivated;

    private ShieldManager shieldManager;
    private void Start() {
        if (!shieldManager) shieldManager = ShieldManager.instance;
        shieldManager.newSide.AddListener(OnNewSide);
    }

    private void OnNewSide(ShieldProtectionSide side) {
        Render(side == attachedSide);
    }

    private void Render(bool activated) {
        onActivated.SetActive(activated);
        onDeactivated.SetActive(!activated);
    }

    public void ActivateLinkedSide() {
        shieldManager.Activate(attachedSide);
    }
}
