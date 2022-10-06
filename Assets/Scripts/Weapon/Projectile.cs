using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private AmmoType _ammoType;
    [SerializeField]
    private int _directHitDamage = 30;
    [SerializeField]
    private bool _isExplosive = false;
    [SerializeField]
    private bool _isGrenade = false;
    [SerializeField]
    private float _grenadeTime = 3.0f;
    private Transform _parent = null;
    private Rigidbody _rigidBody = null;
    private Explosion _explosion = null;

    private bool _isFired = false;
    public AmmoType GetAmmoType() => _ammoType;
    public bool GetIsFired() => _isFired;
    private void Start()
    {
        _parent = this.transform.parent;
        _rigidBody = this.GetComponent<Rigidbody>();
        if (_rigidBody == null)
        {
            _rigidBody = gameObject.AddComponent<Rigidbody>();
        }
        _explosion = FindObjectOfType<Explosion>();
    }

    public void AddForce(Vector3 direction, float speed)
    {
        _rigidBody.Sleep();
        _rigidBody.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Impulse);
        _isFired = true;
        if (_isGrenade)
        {
            Invoke("Explode", _grenadeTime);
        }
    }

    public void Move(Transform location)
    {
        this.transform.position = location.position;
        this.transform.forward = location.forward;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_isGrenade)
        {
            if (other.transform.tag == "Worm")
            {
                Health wormHealth = other.transform.GetComponent<Health>();
                wormHealth.RecieveDamage(_directHitDamage);
            }
            this.transform.parent = _parent;
            _isFired = false;
            gameObject.SetActive(false);
        }
        if (!_isGrenade && _isExplosive)
        {
            Explode();
        }
    }
    private void Explode()
    {
        _explosion.PlayExplosion(this.transform.position);
        gameObject.SetActive(false);
    }
}
