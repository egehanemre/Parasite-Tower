using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRadar : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (target != null)
        {
            transform.rotation = Quaternion.Euler(90, target.rotation.eulerAngles.y, 0);
        }
    }
}
