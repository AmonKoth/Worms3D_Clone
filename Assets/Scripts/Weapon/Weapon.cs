using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private AmmoType _ammoType;
    [SerializeField]
    private float _bulletSpeed = 300.0f;

    private Projectile _ammoPrefab = null;
    //Components
    private Barrel _gunBarrel = null;
    private Ammo _weaponAmmo = null;
    private int _ammocount = 0;

    public void Fire()
    {
        _ammocount = _weaponAmmo.GetAmmoAmount(_ammoType);
        if (_ammocount == 0)
        {
            Debug.Log($"EMPTY");
            return;
        }
        _ammoPrefab.gameObject.SetActive(true);
        _ammoPrefab.Move(_gunBarrel.transform);
        _ammoPrefab.AddForce(_gunBarrel.transform.forward, _bulletSpeed);
        Debug.Log($"FIRE!");
        _weaponAmmo.ReduceAmmo(_ammoType);
    }

    public void Aim(Vector2 aimLocation)
    {
        this.transform.localEulerAngles = aimLocation;
    }
    private void InitializeWeapon()
    {
        _weaponAmmo = GetComponentInParent<Ammo>();
        _gunBarrel = GetComponentInChildren<Barrel>();
        Projectile[] allAmmo = FindObjectsOfType<Projectile>();
        foreach (Projectile projectile in allAmmo)
        {
            if (projectile.GetAmmoType() == _ammoType)
            {
                _ammoPrefab = projectile;
            }
        }
        if (_gunBarrel == null || _weaponAmmo == null || _ammoPrefab == null)
        {
            Debug.Log($"OH no Component missing!!!");
        }
    }

    private void Start()
    {
        InitializeWeapon();
    }
}
