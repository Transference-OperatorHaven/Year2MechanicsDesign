using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{

    [SerializeField] float damage;
    [SerializeField] float m_Lifetime;

    private void Start()
    {
        StartCoroutine(C_Lifetime());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponentInParent<HealthComponent>() != null)
        {
            collision.gameObject.GetComponentInParent<HealthComponent>().ApplyDamage(damage, this);
            Destroy(gameObject);
        }
        else if(collision.gameObject.GetComponentInParent<TilemapCollider2D>() != null)
        {

            Destroy(gameObject);
        }
    }

    IEnumerator C_Lifetime()
    {
        yield return new WaitForSeconds(m_Lifetime);
        Destroy(gameObject);
    }
}
