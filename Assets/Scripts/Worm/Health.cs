using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    //Health Values
    private int _startHealth = 0;
    private float _deathTime = 0f;
    private int _currentHealth = 0;

    public void SetStartHealth(int health) => _startHealth = health;
    public int GetHealth() => _currentHealth;
    public void SetDeathTime(float time) => _deathTime = time;

    //Health Bar
    public event Action<float> OnHealthChanged = delegate { };

    //ded event add
    public event Action<bool> OnDeath = delegate { };

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
        OnDeath(true);
        //gameObject.SetActive(false);
        // Destroy(gameObject);
    }
    private void HealthComponentInitialize()
    {
        _currentHealth = _startHealth;
    }
    private void HazardCollision(bool iscollided)
    {
        if (iscollided)
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
        GetComponent<Worm>().OnHazardCollision += HazardCollision;
        Invoke("HealthComponentInitialize", 0.5f);
    }

}
