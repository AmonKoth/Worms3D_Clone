using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField]
    private int _maxAmmoAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger REgister");
        if (other.gameObject.tag == "Worm")
        {
            int randomNumber = Random.Range(0, 3);
            if (randomNumber == 0)
            {
                other.GetComponent<Ammo>().AddAmmo(AmmoType.BULLETS, Random.Range(1, _maxAmmoAmount + 1));
                Debug.Log($"Bullets Added");
            }
            else if (randomNumber == 1)
            {
                other.GetComponent<Ammo>().AddAmmo(AmmoType.ROCKETS, Random.Range(1, _maxAmmoAmount + 1));
                Debug.Log($"Rockets Added");

            }
            else if (randomNumber == 2)
            {
                other.GetComponent<Ammo>().AddAmmo(AmmoType.GRENADES, Random.Range(1, _maxAmmoAmount + 1));
                Debug.Log($"Grenades Added");
            }
            Destroy(gameObject);
        }
    }
}
