using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TankTarget : MonoBehaviour
{
    public UnityEvent<float, string> onDamageTaken = new UnityEvent<float, string>();
    public string attackSide;

    public void ApplyDamage(float damage) {
        onDamageTaken?.Invoke(damage, attackSide);
    }
}
