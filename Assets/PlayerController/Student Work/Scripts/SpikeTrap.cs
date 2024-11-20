using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float m_Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable victim = collision.GetComponentInParent<IDamagable>();
        if(victim == null ) { return; }
        victim.ApplyDamage(m_Damage, this);
    }
}
