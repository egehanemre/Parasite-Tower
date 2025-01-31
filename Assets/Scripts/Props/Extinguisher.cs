//using System;

using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Extinguisher : MonoBehaviour
{
      [SerializeField] private LayerMask extinguishableLayer; 
      [SerializeField] private float extinguishSpeed = 100;
      [SerializeField] private Transform collisionCheckZone;
      private ObjectsGrabbable grabbableRef;
      private bool extinguishing = false;
      [SerializeField] private ParticleSystem extinguishingEffect;
      private float activationCooldown = 0.5f;
      private bool hasBeenGrabbed = false;
      
      public void Start() {
            Invoke(nameof(DeactivateKinematic), 1);
      }
      
      public void DeactivateKinematic() {
            GetComponent<Rigidbody>().isKinematic = false;
      }

      private void Awake() {
            grabbableRef = GetComponent<ObjectsGrabbable>();
      }

      private void Update() {
            bool newGrabState = grabbableRef.IsGrabbed();
            if (hasBeenGrabbed != newGrabState) {
                  activationCooldown = 0.5f;
                  hasBeenGrabbed = newGrabState;
            }
            
            activationCooldown -= Time.deltaTime;
            
            if(activationCooldown > 0 ) return;

            bool previousExtinguishingState = extinguishing;
            extinguishing = Input.GetKey(KeyCode.Mouse0) && hasBeenGrabbed;
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
