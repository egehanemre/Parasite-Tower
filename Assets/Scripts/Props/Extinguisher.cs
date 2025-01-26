//using System;

using System;
using Unity.VisualScripting;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
      [SerializeField] private LayerMask extinguishableLayer; 
      [SerializeField] private float extinguishRange = 5f;
      [SerializeField] private float extinguishDelay = 0.1f;
      [SerializeField] private float extinguishSpeed = 100;
      private float extinguishTime = 0;
      private ObjectsGrabbable grabbableRef;

      private void Awake() {
            grabbableRef = GetComponent<ObjectsGrabbable>();
      }

      private void Update() {
            extinguishTime -= Time.deltaTime;
            if(extinguishTime > 0) return;

            if (Input.GetKey(KeyCode.E) && grabbableRef.IsGrabbed()) {
                  extinguishTime = extinguishDelay;
                  bool didHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo,
                        extinguishRange, extinguishableLayer);
                  if (didHit && hitInfo.collider.gameObject.TryGetComponent<Fire>(out Fire fire)) {
                        fire.Extinguish(extinguishSpeed*Time.deltaTime);
                  }
            }
      }
}
