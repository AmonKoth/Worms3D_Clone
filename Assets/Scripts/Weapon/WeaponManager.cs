using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private AmmoType _currentWeapon = AmmoType.BULLETS;
    private Weapon _activeweapon = null;
    private AmmoText _ammotext = null;
    private bool _canSwitch = true;
    public bool GetCanSwitch() => _canSwitch;


    //try events for firing and fire return etcetc
    private Dictionary<AmmoType, Weapon> _weaponList = new Dictionary<AmmoType, Weapon>();

    private void InitializeWeaponManager()
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            if (!_weaponList.ContainsKey(weapon.GetAmmoType()))
            {
                _weaponList.Add(weapon.GetAmmoType(), weapon);
            }
        }
        _ammotext = FindObjectOfType<AmmoText>();
        StartCoroutine("OnGameStart", _currentWeapon);
    }

    public void SwitchWeapon(AmmoType ammoType)
    {
        Weapon weaponToEnable = null;
        Weapon previousWeapon = null;
        if (_weaponList.TryGetValue(ammoType, out weaponToEnable))
        {
            if (_weaponList.TryGetValue(_currentWeapon, out previousWeapon))
            {
                previousWeapon.gameObject.SetActive(false);
                weaponToEnable.gameObject.SetActive(true);
                _currentWeapon = weaponToEnable.GetAmmoType();
                _activeweapon = weaponToEnable;
                _ammotext.SetAmmoText(_activeweapon.GetAmmoCount().ToString());
            }
        }
    }

    IEnumerator OnGameStart(AmmoType ammoType)
    {
        yield return new WaitForSeconds(0.3f);
        foreach (Weapon weapon in GetComponentsInChildren<Weapon>())
        {
            if (weapon.GetAmmoType() != ammoType)
            {
                weapon.gameObject.SetActive(false);
            }
            if (weapon.GetAmmoType() == ammoType)
            {
                _ammotext.SetAmmoText(weapon.GetAmmoCount().ToString());
            }
        }
    }
    public void Aim(Vector2 aimLocation)
    {
        this.transform.localEulerAngles = aimLocation;
    }
    public AmmoType GetActiveWeaponType() => _activeweapon.GetAmmoType();
    public void Fire()
    {
        _activeweapon.Fire();
        _ammotext.ClearText();
    }

    private void Start()
    {
        InitializeWeaponManager();
        SwitchWeapon(_currentWeapon);
    }


}
