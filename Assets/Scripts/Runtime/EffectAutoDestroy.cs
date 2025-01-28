using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
    [SerializeField] private float time;

    private void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        if (time < 0) {
            Destroy(gameObject);
        }
    }
}
