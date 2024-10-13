using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

	[SerializeField] private StatefulRaycastSensor2D m_GroundSensor;
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;
	[SerializeField] private int m_totalJumps;
	[SerializeField] private int m_JumpCount;
    [Header("Jump Buffer Settings")]
    [SerializeField] private float m_JumpBufferTime;
	private float m_JumpBufferCountDown;
	private Coroutine m_JumpBufferCoroutine;
	[Header("Jump Apex Anti-Gravity Settings")]
	private Coroutine m_AntiGravCheckCoroutine;

    [Header("Coyote Time Settings")]
    [SerializeField] private float m_CoyoteTime;
	private float m_CoyoteTimeCountDown;
	private Coroutine m_CoyoteCoroutine;

	private void Awake()
	{
		m_RB = GetComponent<Rigidbody2D>();
		Debug.Assert(m_GroundSensor != null);
	}

	private void Jump()
	{
        m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
        m_JumpCount++;
        m_JumpBufferCountDown = 0f;
		if(m_AntiGravCheckCoroutine == null)
		{
			m_AntiGravCheckCoroutine = StartCoroutine(AntiGravApex());
        }
		if (m_JumpBufferCoroutine != null)
        {
            StopCoroutine(m_JumpBufferCoroutine);
            m_JumpBufferCoroutine = null;
        }
    }

	public void SetInMove(float newMove) => m_RB.linearVelocityX = m_MoveSpeed * newMove;

	public void StartJump()
	{
		if (m_JumpCount < m_totalJumps)
		{
			if (m_GroundSensor.HasDetectedHit() || m_CoyoteTimeCountDown > 0f)
			{
				Jump();
			}
		}
		else
		{
			if (m_JumpBufferCoroutine == null)
			{
				m_JumpBufferCountDown = m_JumpBufferTime;
				m_JumpBufferCoroutine = StartCoroutine(JumpBuffer());
			}
			if(m_JumpBufferCoroutine != null && m_JumpBufferCountDown <= 0f)
			{
                StopCoroutine(m_JumpBufferCoroutine);
                m_JumpBufferCoroutine = null;
				m_JumpBufferCountDown = m_JumpBufferTime;
                m_JumpBufferCoroutine = StartCoroutine(JumpBuffer());
            }
		}
	}

	public void StopJump() 
	{
		
	}

	IEnumerator JumpBuffer()
	{
        while (m_JumpBufferCountDown > 0f)
        {
            m_JumpBufferCountDown -= Time.deltaTime;
            if (m_GroundSensor.HasDetectedHit())
            {
				Jump();
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

	IEnumerator AntiGravApex()
	{
		while(!m_GroundSensor.HasDetectedHit())
		{
			if(m_RB.linearVelocityY > -1f && m_RB.linearVelocityY < 1f)
			{
				Debug.Log("jump apex");
			}
			yield return null;
		}
	}

	private void FixedUpdate()
	{
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if(m_GroundSensor.HasDetectedHit())
		{

			m_JumpCount = 0;
			if(m_CoyoteCoroutine != null)
			{
                StopCoroutine(m_CoyoteCoroutine);
				m_CoyoteCoroutine = null;
            }

			if(m_AntiGravCheckCoroutine != null)
			{
				StopCoroutine(m_AntiGravCheckCoroutine);
				m_AntiGravCheckCoroutine = null;
			}
			
		}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
		//Debug.Log(m_mostRecentContactY + " + " + gameObject.transform.position.y);
		
		if (m_RB.linearVelocityY < 0)
		{
			
            if (!m_GroundSensor.HasDetectedHit() && m_CoyoteCoroutine == null)
			{
                m_CoyoteTimeCountDown = m_CoyoteTime;
                Debug.Log("setting coyote time");
                m_CoyoteCoroutine = StartCoroutine(CoyoteTimeReduction());
            }
        }
        
    }

    IEnumerator CoyoteTimeReduction()
    {
        while (m_CoyoteTimeCountDown > 0f)
        {
            m_CoyoteTimeCountDown -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
}
