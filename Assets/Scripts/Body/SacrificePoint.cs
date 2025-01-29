using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificePoint : MonoBehaviour
{
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private List<ParticleSystem> bloodEffects = new List<ParticleSystem>();

    public void Sacrifice(GameObject skullObject) {
        upgradeManager.Money++;
        upgradeManager.UpdateMoneyText();
        Destroy(skullObject);
        foreach (var bloodEffect in bloodEffects) {
            bloodEffect.Play();
        }
    }
}
