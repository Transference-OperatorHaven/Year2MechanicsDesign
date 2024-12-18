using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class HealthComponent : MonoBehaviour, IDamagable
{

    public event Action<float, float, float, GameObject> onDamaged;
    public event Action<MonoBehaviour> onDead;

    [SerializeField] private float m_MaxHealth;
    private float m_CurrentHealth;

    [Header("Invulnerability Settings")]
    private bool m_Invulernable;
    public float m_InvulTimer = 0.5f;
    private Coroutine m_InvulCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public float GetMaxHealth() { return m_MaxHealth; }

    public float GetCurrentHealth() { return m_CurrentHealth; }

    public void SetHealth(float health)
    {
        m_CurrentHealth = health;
        onDamaged?.Invoke(m_CurrentHealth, m_MaxHealth, 100, this.gameObject);
    }

    public void ApplyDamage(float damage, MonoBehaviour attacker)
    {
        if (m_Invulernable) return;
        if(m_InvulCoroutine != null)
        {
            StopCoroutine(m_InvulCoroutine);
            m_InvulCoroutine = null;
        }
        float change = Mathf.Min(m_CurrentHealth, damage);
        m_CurrentHealth -= change;

        onDamaged?.Invoke(m_CurrentHealth, m_MaxHealth, change, attacker.gameObject);
        if (m_CurrentHealth <= 0) { onDead?.Invoke(attacker); }
        if (m_InvulCoroutine == null)
        {
             m_InvulCoroutine = StartCoroutine(C_Invulnerable());
        }
        
    }

    IEnumerator C_Invulnerable()
    {
        m_Invulernable = true;
        yield return new WaitForSeconds(m_InvulTimer);
        m_Invulernable = false;
    }
}
