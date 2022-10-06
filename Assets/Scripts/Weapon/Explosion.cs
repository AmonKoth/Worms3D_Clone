using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float _explosionTime = 1.0f;
    [SerializeField]
    private int _explosiveDamage = 20;

    private ParticleSystem _particleSystem;
    public int GetExplosiveDamage() => _explosiveDamage;
    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        Deactivate();
    }

    public void PlayExplosion(Vector3 location)
    {
        this.transform.position = location;
        var emission = _particleSystem.emission;
        emission.enabled = true;
        Invoke("Deactivate", _explosionTime);
    }

    private void Deactivate()
    {
        var emission = _particleSystem.emission;
        emission.enabled = false;
    }
}
