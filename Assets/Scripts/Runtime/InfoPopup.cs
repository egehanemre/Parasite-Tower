using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InfoPopup : MonoBehaviour
{
    public bool isEnabled = false;
    public GameObject gmeObject;
    void Update() {
        if (isEnabled && !Input.GetKey(KeyCode.Tab)) {
            isEnabled = false;
            gmeObject.SetActive(false);
        }
        else if (!isEnabled && Input.GetKey(KeyCode.Tab)) {
            isEnabled = true;
            gmeObject.SetActive(true);
        }
    }
}
