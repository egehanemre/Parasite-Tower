using UnityEngine;

public class CompassLabelFixer : MonoBehaviour
{
    public RectTransform[] compassLabels; 

    void LateUpdate()
    {
        foreach (RectTransform label in compassLabels)
        {
            label.localRotation = Quaternion.identity;
        }
    }
}
