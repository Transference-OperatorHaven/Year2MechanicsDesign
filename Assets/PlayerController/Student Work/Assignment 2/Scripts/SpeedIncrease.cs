using UnityEngine;

public class SpeedIncrease : MonoBehaviour
{

    [SerializeField] private float m_MoveMult;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<CharacterMovement>().m_MoveSpeed *= m_MoveMult;
        }
        
    }
}
