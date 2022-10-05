using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _life;

    private void Awake()
    {
        GetComponentInParent<Health>().OnHealthChanged += HandleHealthChange;
        GetComponentInParent<PlayerController>().HandleColor += HandleColor;
    }

    private void HandleColor(Color color)
    {
        _life.color = color;
    }

    private void HandleHealthChange(float health)
    {
        _life.fillAmount = health;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

}
