using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    //pick a random spot on the map
    //tie the spawn to number of turns
    [SerializeField]
    private AmmoPickup _ammoPickup = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AmmoSpawn());
    }
    IEnumerator AmmoSpawn()
    {
        var ammo = Instantiate(_ammoPickup, this.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(3f);
        StartCoroutine(AmmoSpawn());
    }

}
