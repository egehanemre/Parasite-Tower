using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificePoint : MonoBehaviour
{
    [SerializeField] private UpgradeManager upgradeManager;

    public void Sacrifice(GameObject skullObject) {
        upgradeManager.Money++;
        Destroy(skullObject);
    }
}
