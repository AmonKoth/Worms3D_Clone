using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private AmmoType _ammoType;
    [SerializeField]
    private float _projectileSpeed = 300.0f;

    private Projectile _ammoPrefab = null;
    //Components
    private Barrel _gunBarrel = null;
    private Ammo _weaponAmmo = null;
    private int _ammocount = 0;

    public AmmoType GetAmmoType() => _ammoType;

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
        _ammoPrefab.AddForce(_gunBarrel.transform.forward, _projectileSpeed);
        Debug.Log($"FIRE!");
        _weaponAmmo.ReduceAmmo(_ammoType);
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
        if (_gunBarrel == null)
        {
            Debug.Log($"OH no gunbarrel missing!!!");
            Debug.Log(this.name);
        }
        if (_weaponAmmo == null)
        {
            Debug.Log($"OH no ammo in parent missing!!!");
            Debug.Log(this.name);
        }
        if (_ammoPrefab == null)
        {
            Debug.Log($"OH no ammoprefab missing!!!");
            Debug.Log(this.name);
        }
    }

    private void Start()
    {
        InitializeWeapon();
    }
}
