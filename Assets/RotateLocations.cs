using UnityEngine;

public class RotateLocations : MonoBehaviour
{
    public GameObject MainRotation; 

    private void Update()
    {
        float parentZRotation = MainRotation.transform.rotation.eulerAngles.z;

        float oppositeZRotation = -parentZRotation;

        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, oppositeZRotation);
    }
}
