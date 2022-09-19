using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private AmmoType _ammotType;


    private int _ammocount = 0;
    public void Fire()
    {
        if (_ammocount == 0)
        {
            Debug.Log($"EMPTY");
            return;
        }
        Debug.Log($"KACHOW");
        _ammocount--;
    }

    public void Aim(Vector2 aimLocation)
    {
        this.transform.localEulerAngles = aimLocation;
    }
}
