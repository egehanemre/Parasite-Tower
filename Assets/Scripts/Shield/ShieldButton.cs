using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldButton : MonoBehaviour
{

    [SerializeField] private ShieldProtectionSide attachedSide;
    private ShieldManager shieldManager;
    private void Start() {
        if (!shieldManager) shieldManager = FindObjectOfType<ShieldManager>();
    }

    public void ActivateLinkedSide() {
        shieldManager.Activate(attachedSide);
    }
}
