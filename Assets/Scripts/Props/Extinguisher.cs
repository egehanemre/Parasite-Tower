//using System;

using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Extinguisher : MonoBehaviour
{
      [SerializeField] private LayerMask extinguishableLayer; 
      [SerializeField] private float extinguishRange = 5f;
      [SerializeField] private float extinguishSpeed = 100;
      [SerializeField] private Transform collisionCheckZone;
      private ObjectsGrabbable grabbableRef;
      private bool extinguishing = false;
      [SerializeField] private ParticleSystem extinguishingEffect;

      private void Awake() {
            grabbableRef = GetComponent<ObjectsGrabbable>();
      }
      
      private void Update() {
            bool previousExtinguishingState = extinguishing;
            extinguishing = Input.GetKey(KeyCode.E) && grabbableRef.IsGrabbed();
            if (previousExtinguishingState != extinguishing) {
                  if (extinguishing)
                  {
                        extinguishingEffect.gameObject.SetActive(true);
                        extinguishingEffect.Play();
                  }
                  else extinguishingEffect.Stop();
            }
      }

      private void FixedUpdate()
      {
            if (extinguishing) {
                  Collider[] overlaps = Physics.OverlapBox(collisionCheckZone.position, collisionCheckZone.lossyScale/2, Quaternion.identity, extinguishableLayer);
                  foreach (var overlap in overlaps) {
                        if (overlap.gameObject.TryGetComponent<Fire>(out Fire fire)) {
                              fire.Extinguish(extinguishSpeed*Time.fixedDeltaTime);
                        }   
                  }
            }
      }
}
