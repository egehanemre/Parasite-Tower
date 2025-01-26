//using UnityEngine;

//public class PowerSupply : MonoBehaviour, IEvent, IInteractable
//{
//    [SerializeField] private GameObject connected; 
//    [SerializeField] private float possibility = 1; 
//    [SerializeField] private float fixingCooldown = 0.33f; 
//    [SerializeField] private Vector3 radarOffset = Vector3.zero; 

//    private IPowerDependent dependent; 

//    private void Start()
//    {
//        if (!connected)
//        {
//            Debug.LogWarning("No object is connected to this power supply!", gameObject);
//            return;
//        }

//        dependent = connected.GetComponent<IPowerDependent>();

//        if (dependent == null)
//        {
//            Debug.LogError("The connected object does not implement IPowerDependent!", connected);
//        }
//    }
//    public void Tick()
//    {
//        if (dependent == null)
//        {
//            Debug.LogWarning("Power supply connection is missing or incorrect.", gameObject);
//            return;
//        }

//        dependent.SetPowerState(false);
//    }
//    public void Fix()
//    {
//        if (dependent != null)
//        {
//            dependent.SetPowerState(true);
//            Debug.Log("Power restored to the connected object.", gameObject);
//        }
//    }
//    public float GetWeight()
//    {
//        return possibility;
//    }
//    public bool IsActive()
//    {
//        return dependent != null && !dependent.GetPowerState();
//    }
//    public bool CanHold(out float time)
//    {
//        time = fixingCooldown;
//        return true;
//    }
//    public void InteractOnHolding(Transform pov, float holdingTime)
//    {
//        if (holdingTime >= fixingCooldown)
//        {
//            Fix();
//        }
//    }
//    public void Interact()
//    {
//        Debug.Log("Interacted with PowerSupply.");
//    }
//    public void SetPhysicsMode(bool isActive)
//    {
//        Debug.LogWarning("SetPhysicsMode is not implemented for PowerSupply.");
//    }
//    public bool CanGrab()
//    {
//        return true;
//    }
//}
