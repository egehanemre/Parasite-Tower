//using System;

using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Extinguisher : MonoBehaviour
{
      [SerializeField] private LayerMask extinguishableLayer; 
      [SerializeField] private float extinguishRange = 5f;
      [SerializeField] private float extinguishDelay = 0.1f;
      [SerializeField] private float extinguishSpeed = 100;
      [SerializeField] private float spread = 0.3f;
      [SerializeField] private Transform startLocation;
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
                  Vector3 rightSpread = transform.right * spread;
                  Vector3 upSpread = transform.up * spread;
                  if (Random.Range(0f, 1f) > 0.5f) rightSpread *= -1;
                  if (Random.Range(0f, 1f) > 0.5f) upSpread *= -1;
                  Vector3 spreadPosition = rightSpread + upSpread;
                        
                  bool didHit = Physics.Raycast(startLocation.position, transform.forward + spreadPosition, out RaycastHit hitInfo,
                        extinguishRange, extinguishableLayer);
                  //Debug.DrawRay(startLocation.position, transform.forward+spreadPosition, Color.blue, 0.25f);
                  if (didHit && hitInfo.collider.gameObject.TryGetComponent<Fire>(out Fire fire)) {
                        fire.Extinguish(extinguishSpeed*Time.deltaTime);
                  }
            }
      }
}
