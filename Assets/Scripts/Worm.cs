using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private float _health = 100.0f;
    [SerializeField]
    private float _deathTime = 3.0f;

    private float _currentHealth = 0f;

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

    //Health Functions
    public void SetHealth(float health) { _currentHealth = health; }
    public float GetHealt() => _currentHealth;
    public void RecieveDamage(float damage)
    {
        if (_rigidBody.IsSleeping())
        {
            _rigidBody.WakeUp();
        }
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Invoke("HandleDeath", _deathTime);
        }
    }
    public void Heal(float heal) { _currentHealth += heal; }


    private void HandleDeath()
    {
        gameObject.SetActive(false);
    }
    public void HandleMovement(Vector2 dir)
    {
        Vector3 move = new Vector3(dir.x, 0.0f, dir.y);
        _rigidBody.AddForce(move * _moveSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
    }
    public void Jump()
    {
        if (_onGround)
        {
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
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
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void Aim(float lookDir)
    {
        Vector2 rotate = new Vector2(0.0f, lookDir);
        this.transform.localEulerAngles = rotate;
    }

    private void InitializeWorm()
    {
        _currentHealth = _health;
        _curSpeed = _moveSpeed * Time.fixedDeltaTime;
        _rigidBody = this.GetComponent<Rigidbody>();
        if (_rigidBody == null)
        {
            Debug.Log($"SACRE BLEU");
            return;
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
