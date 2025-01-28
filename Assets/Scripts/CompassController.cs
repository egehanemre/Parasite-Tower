using UnityEngine;

public class CompassSync : MonoBehaviour
{
    public RectTransform compass;  // Assign the RectTransform for the compass
    public Transform target3D;     // Assign the 3D object (e.g., player or radar object)

    void Update()
    {
        float yaw = target3D.eulerAngles.y;

        compass.rotation = Quaternion.Euler(0, 0, -yaw);
    }
}
