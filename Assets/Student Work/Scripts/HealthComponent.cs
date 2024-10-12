using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamagable
{

    public event Action<float, float, float> onDamaged;
    public event Action<MonoBehaviour> onDead;

    [SerializeField] private float m_MaxHealth;
    private float m_CurrentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void ApplyDamage(float damage, MonoBehaviour attacker)
    {
        float change = Mathf.Min(m_CurrentHealth, damage);
        m_CurrentHealth -= change;

        onDamaged?.Invoke(m_CurrentHealth, m_MaxHealth, change);
        if (m_CurrentHealth <= 0) { onDead?.Invoke(attacker); }
    }
}
