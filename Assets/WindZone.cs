using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    private Coroutine m_WindPushCoroutine;
    private CharacterMovement m_PlayerCharacterMovement;
    private Rigidbody2D m_PlayerRigidbody;
    [SerializeField] private Vector2 m_Direction;
    [SerializeField] private float m_Strength;

    void OnTriggerEnter2D(Collider2D collider)
    {
        
        if(collider.gameObject.layer == 6)
        {
            
            if (m_WindPushCoroutine == null)
            {
                m_PlayerCharacterMovement = collider.gameObject.GetComponentInParent<CharacterMovement>();
                m_PlayerRigidbody = collider.gameObject.GetComponentInParent<Rigidbody2D>();
                m_WindPushCoroutine = StartCoroutine(C_WindPush());
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        if(m_WindPushCoroutine != null)
        {
            StopCoroutine(m_WindPushCoroutine);
            m_WindPushCoroutine = null;
        }
    }

    IEnumerator C_WindPush()
    {
        if(!m_PlayerCharacterMovement.m_CrouchCheck)
        {
            m_PlayerRigidbody.AddForce(m_Direction * m_Strength, ForceMode2D.Impulse);
        }
        yield return null;
    }
}
