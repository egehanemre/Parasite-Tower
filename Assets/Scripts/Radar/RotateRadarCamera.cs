using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRadarCamera : MonoBehaviour
{
    public GameObject playerTarget;

    void Update()
    {
        Vector3 currentRotation = gameObject.transform.rotation.eulerAngles; 
        gameObject.transform.rotation = Quaternion.Euler(currentRotation.x, playerTarget.transform.rotation.eulerAngles.y, currentRotation.z); 
    }
}
