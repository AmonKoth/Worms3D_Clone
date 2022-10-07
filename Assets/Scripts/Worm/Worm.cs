using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Ammo))]
public class Worm : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private int _health = 100;
    [SerializeField]
    private float _deathTime = 2.0f;

    [Header("Movement")]
    [SerializeField]
    private float _moveSpeed = 1.0f;
    [SerializeField]
    private float _JumpPower = 5.0f;

    private float _curSpeed = 0.0f;
    private bool _onGround = true;
    private float _groundDistance = 0.2f;
    //Components
    private Rigidbody _rigidBody = null;
    private WeaponManager _weapons = null;
    private Health _wormHealth = null;
    //Camera
    private CameraManager _cameraManager = null;
    private bool _isActive = false;
    private bool _isDead = false;

    public bool GetIsActive() => _isActive;
    public void SetIsActive(bool activity) { _isActive = activity; }
    public bool GetIsDead() => _isDead;
    public void SetIsDead(bool dead) { _isDead = dead; }

    //Weapon
    private bool _isFired = false;
    public bool GetIsFired() => _isFired;

    public void HandleMovement(Vector2 dir)
    {
        if (dir.y > 0)
        {
            _rigidBody.AddForce(this.transform.forward * _moveSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        else if (dir.y < 0)
        {
            _rigidBody.AddForce(-this.transform.forward * _moveSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        if (dir.x > 0)
        {
            _rigidBody.AddForce(this.transform.right * _moveSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        else if (dir.x < 0)
        {
            _rigidBody.AddForce(-this.transform.right * _moveSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    public void Jump()
    {
        if (_onGround)
        {
            _rigidBody.AddForce(Vector3.up * _JumpPower, ForceMode.Impulse);
            _onGround = false;
        }
    }

    private void Grounder()
    {
        RaycastHit hit;
        Ray groundCheck = new Ray(transform.position + Vector3.up * _groundDistance, -Vector3.up);
        if (Physics.Raycast(groundCheck, out hit, _groundDistance))
        {
            _onGround = true;
        }
    }

    public void Aim(Vector2 lookDir)
    {
        Vector2 rotate = new Vector2(0.0f, lookDir.y);
        this.transform.localEulerAngles = rotate;
        Vector2 aimDir = new Vector2(lookDir.x, 0);
        _weapons.Aim(aimDir);
    }

    public void TurnStart()
    {
        if (_rigidBody.IsSleeping())
        {
            _rigidBody.WakeUp();
        }
        //        Debug.Log($"YES SIR!");
        _isFired = false;
        _isActive = true;
        _cameraManager.SetTarget(this.transform);
    }
    public void SetActiveWeapon(AmmoType ammoType)
    {
        _weapons.SwitchWeapon(ammoType);
    }

    private void InitializeWorm()
    {
        //Health
        HealthInitialize();
        _curSpeed = _moveSpeed * Time.fixedDeltaTime;
        _rigidBody = this.GetComponent<Rigidbody>();
        _cameraManager = FindObjectOfType<CameraManager>();
        _weapons = this.GetComponentInChildren<WeaponManager>();
        GetComponentInParent<PlayerController>().SwitchingWorm += WormSwitched;
        if (_rigidBody == null)
        {
            Debug.Log($"RigidBody Missing");

            return;
        }
    }
    private void WormSwitched(String worm)
    {
        if (worm == this.transform.name)
        {
            _cameraManager.SetTarget(this.transform);
        }
    }
    private void HealthInitialize()
    {
        _wormHealth = GetComponent<Health>();
        if (_wormHealth == null)
        {
            this.gameObject.AddComponent<Health>();
        }
        _wormHealth.SetStartHealth(_health);
        _wormHealth.SetDeathTime(_deathTime);
        _wormHealth.OnDeath += WormDied;
        _wormHealth.OnExplosiveDamage += ExplosionPhysics;
    }
    private void ExplosionPhysics(GameObject explosion)
    {
        _rigidBody.AddExplosionForce(5, explosion.transform.position, 3, 3, ForceMode.Impulse);
    }

    public void Fire()
    {
        if (!_isFired)
        {
            _weapons.Fire();
            _isFired = true;
        }
    }
    private void WormDied(bool dead)
    {
        foreach (Transform transform in GetComponentsInChildren<Transform>())
        {
            if (transform != this.transform)
            {
                transform.gameObject.SetActive(false);
            }
        }
        if (dead)
        {
            _rigidBody.constraints = RigidbodyConstraints.FreezePosition;
            _isDead = true;
            _isActive = false;
        }
    }

    private void Start()
    {
        InitializeWorm();
    }

    private void Update()
    {
        if (!_onGround)
        {
            Grounder();
        }
    }
}
