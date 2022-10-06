using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    //pick a random spot on the map
    //tie the spawn to number of turns
    [SerializeField]
    private AmmoPickup _ammoPickup = null;
    [SerializeField]
    private float _maxCheckDistance = 20f;
    [SerializeField]
    private float _maxXBounds = 20f;
    [SerializeField]
    private float _maxZBounds = 20f;
    [SerializeField]
    private int _maxSpawnTurn = 3;

    private int _turnCount = 0;
    private int _spawnTurn = 1;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<TurnManager>().TurnPassed += IncreaseTurnCount;
        _spawnTurn = Random.Range(1, _maxSpawnTurn + 1);
    }

    private void SpawnAmmo()
    {
        PickRandomLocation();
        RaycastHit groundHit;
        Ray groundCheck = new Ray(transform.position, Vector3.down * _maxCheckDistance);
        if (Physics.Raycast(groundCheck, out groundHit, _maxCheckDistance))
        {
            if (groundHit.transform.tag == "Ground")
            {
                var ammo = Instantiate(_ammoPickup, this.transform.position, Quaternion.identity);
                _turnCount = 0;
                _spawnTurn = Random.Range(1, _maxSpawnTurn + 1);
            }
            else
            {
                SpawnAmmo();
            }
        }
    }
    private void PickRandomLocation()
    {
        this.transform.position = new Vector3(Random.Range(-_maxXBounds, _maxXBounds + 1), 10, Random.Range(-_maxZBounds, _maxZBounds + 1));
    }
    private void IncreaseTurnCount(bool turnIncreased)
    {
        if (turnIncreased)
        {
            _turnCount++;
        }
        if (_turnCount == _spawnTurn)
        {
            SpawnAmmo();
        }
    }
}
