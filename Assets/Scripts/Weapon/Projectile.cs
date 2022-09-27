using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private AmmoType _ammoType;
    private Transform _parent = null;
    private Rigidbody _rigidBody = null;

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
    }

    public void AddForce(Vector3 direction, float speed)
    {
        _rigidBody.Sleep();
        _rigidBody.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Impulse);
        _isFired = true;
    }

    public void Move(Transform location)
    {
        this.transform.position = location.position;
        this.transform.forward = location.forward;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Worm")
        {
            Worm health = other.transform.GetComponent<Worm>();
            health.RecieveDamage(20);
        }
        this.transform.parent = _parent;
        _isFired = false;
        gameObject.SetActive(false);
    }
}
