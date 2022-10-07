using System;
using UnityEngine;

[RequireComponent(typeof(Worm))]
public class Health : MonoBehaviour
{
    //Health Values
    private int _startHealth = 0;
    private float _deathTime = 0f;
    private int _currentHealth = 0;



    public void SetStartHealth(int health) => _startHealth = health;
    public int GetHealth() => _currentHealth;
    public void SetDeathTime(float time) => _deathTime = time;

    private Explosion _explosion = null;
    //Health Bar
    public event Action<float> OnHealthChanged = delegate { };
    public event Action<bool> OnDeath = delegate { };
    public event Action<GameObject> OnExplosiveDamage = delegate { };

    public void RecieveDamage(int damage)
    {
        Debug.Log($"You'll regret that!");
        _currentHealth -= damage;
        HealthModified();
        if (_currentHealth <= 0)
        {
            Invoke("HandleDeath", _deathTime);
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        int damage = other.GetComponent<Explosion>().GetExplosiveDamage();
        RecieveDamage(damage);
        OnExplosiveDamage(other);
    }

    void Heal(int heal)
    {
        _currentHealth += heal;
        if (_currentHealth > _startHealth)
        {
            _currentHealth = _startHealth;
        }
        HealthModified();
    }
    private void HandleDeath()
    {
        Debug.Log($"BYE BYE");
        this.GetComponent<CapsuleCollider>().enabled = false;
        _explosion.PlayExplosion(this.transform.position);
        OnDeath(true);
    }
    private void HealthComponentInitialize()
    {
        _currentHealth = _startHealth;
        _explosion = FindObjectOfType<Explosion>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            _currentHealth = 0;
            HealthModified();
            HandleDeath();
        }
    }
    //Handles the healthbar stuff
    private void HealthModified()
    {
        float healthPercentage = (float)_currentHealth / (float)_startHealth;
        OnHealthChanged(healthPercentage);
    }
    private void Start()
    {
        Invoke("HealthComponentInitialize", 0.5f);
    }

}
