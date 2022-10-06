using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] AmmoSlot[] ammoSlots;
    [System.Serializable]
    private class AmmoSlot
    {
        public AmmoType ammoType;
        public int ammoCount;
    }

    private AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach (AmmoSlot ammoSlot in ammoSlots)
        {
            if (ammoSlot.ammoType == ammoType)
            {
                return ammoSlot;
            }
        }
        return null;
    }
    public void ReduceAmmo(AmmoType ammoType) { GetAmmoSlot(ammoType).ammoCount--; }
    public void AddAmmo(AmmoType ammoType, int count) { GetAmmoSlot(ammoType).ammoCount += count; }
    public int GetAmmoAmount(AmmoType ammoType) => GetAmmoSlot(ammoType).ammoCount;

}
