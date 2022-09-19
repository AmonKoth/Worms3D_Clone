using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] AmmoSlot[] ammoSlots;

    [System.Serializable]
    private class AmmoSlot
    {
        [SerializeField]
        private AmmoType _ammoType;
        [SerializeField]
        private int _ammoCount;

        public AmmoType GetAmmoType() => _ammoType;
        public int GetAmmoCount() => _ammoCount;

        public void AddAmmo(int count) { _ammoCount += count; }
        public void ReduceAmmo(int count) { _ammoCount -= count; }

    }

}
