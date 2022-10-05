using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWheel : MonoBehaviour
{
    private PlayerController[] _players;


    // Start is called before the first frame update
    void Awake()
    {
        _players = FindObjectsOfType<PlayerController>();
        Invoke("Deactivate", 0.04f);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public void BulletActiveAmmo()
    {
        WeaponSwitch(AmmoType.BULLETS);
        Deactivate();
    }
    public void RocketsActiveAmmo()
    {
        WeaponSwitch(AmmoType.ROCKETS);
        Deactivate();
    }
    public void GrenadeActiveAmmo()
    {
        WeaponSwitch(AmmoType.GRENADES);
        Deactivate();
    }

    private void WeaponSwitch(AmmoType ammoType)
    {
        foreach (PlayerController player in _players)
        {
            if (player.GetTurn())
            {
                player.WeaponSwitch(ammoType);
            }
        }
    }
}
