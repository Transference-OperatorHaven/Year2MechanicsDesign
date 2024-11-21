using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<HealthComponent>().ApplyDamage(100, this);
        }
        
    }
}
