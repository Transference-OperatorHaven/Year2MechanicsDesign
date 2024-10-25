using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;
	private CapsuleCollider2D m_Collider;

	[SerializeField] private StatefulRaycastSensor2D m_GroundSensor;
    [SerializeField] private float m_MoveSpeedBase;
    private float m_MoveSpeed;
    private float m_MoveDir;
	[SerializeField] private float m_JumpStrength;
	[SerializeField] private int m_totalJumps;
	[SerializeField] private int m_JumpCount;
	private bool m_JumpState;
	[Header("Fall Through Semi Solids")]
	private Coroutine m_FallThroughCoroutine;
	private Collider2D m_PlatformCollision;
	
	
	[Header("Crouch Settings")]
    [SerializeField] private float m_CrouchLedgeCheckDistance;
    [SerializeField] private float m_CrouchMoveSpeedMult;
	private float m_LockedXPosition;
	private float m_TempMoveSpeed;
    private Coroutine m_CrouchCoroutine;
	private bool m_CrouchCheck;
	[SerializeField] private float m_CrouchLimit;
    private SpriteRenderer m_SpriteRenderer;
    [Header("Crouch Buffer")]
    [SerializeField] private float m_CrouchBufferTime;
    private float m_CrouchBufferCountDown;
    private Coroutine m_CrouchBufferCoroutine;


    [Header("Fall Speed Cap Variables")]
    [SerializeField] private float m_FallSpeedCap;
	private Coroutine m_FallingCoroutine;
    
	[Header("Jump Buffer Settings")]
    [SerializeField] private float m_JumpBufferTime;
	private float m_JumpBufferCountDown;
	private Coroutine m_JumpBufferCoroutine;

	[Header("Apex Anti-Gravity Settings")]
	private Coroutine m_AntiGravCheckCoroutine;
	private float m_AntiGravCountDown;
	[SerializeField] private float m_AntiGravTimer;

    [Header("Coyote Time Settings")]
    [SerializeField] private float m_CoyoteTime;
	private float m_CoyoteTimeCountDown;
	private Coroutine m_CoyoteCoroutine;

    private void Awake()
	{
		m_RB = GetComponent<Rigidbody2D>();
		m_Collider = GetComponentInChildren<CapsuleCollider2D>();
		m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		Debug.Assert(m_GroundSensor != null);
		m_MoveSpeed = m_MoveSpeedBase;
	}

	private void Jump()
	{
		if (m_RB.linearVelocityY + m_JumpStrength <= m_JumpStrength*1.5)
		{
			CancelCrouch();
			m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
			m_JumpCount++;
			if(m_JumpBufferCountDown > 0)
			{
				JumpCancel();
			}
			m_JumpBufferCountDown = 0f;
			if (m_AntiGravCheckCoroutine == null)
			{
				m_AntiGravCountDown = m_AntiGravTimer;
				m_AntiGravCheckCoroutine = StartCoroutine(C_AntiGravApex());
			}
			if (m_JumpBufferCoroutine != null)
			{
				StopCoroutine(m_JumpBufferCoroutine);
				m_JumpBufferCoroutine = null;
			}
		}
    }

	public void JumpCancel()
	{
        if (!m_GroundSensor.HasDetectedHit() || (m_GroundSensor.GetCollider() != null && !m_Collider.IsTouching(m_GroundSensor.GetCollider())))
        {
            if (m_AntiGravCheckCoroutine != null)
            {
                StopCoroutine(m_AntiGravCheckCoroutine);
                m_AntiGravCheckCoroutine = null;
            }
            if (m_RB.linearVelocityY > 0f)
            {
                m_RB.linearVelocityY = 0f;
            }
            if (m_FallingCoroutine == null)
            {
                m_FallingCoroutine = StartCoroutine(C_FallingCoroutine());
            }

        }
    }

    public void SetInMove(float newMove)

	{
		m_MoveDir = newMove;
		m_RB.linearVelocityX = m_MoveSpeed * newMove;
	}

	public void FallDown()
	{
		if(m_GroundSensor.HasDetectedHit() && !m_CrouchCheck)
		{
            Collider2D platform = m_GroundSensor.GetCollider();
            if (platform.tag == "SemiSolid" && platform != null)
            {
				m_PlatformCollision = platform;
				m_PlatformCollision.GetComponent<PlatformEffector2D>().rotationalOffset = 180;
            }
        }
		
	}

	public void FallDownCancel()
	{
		if(m_PlatformCollision != null)
		{
			m_FallThroughCoroutine = StartCoroutine(C_StopFallDown());
        }

    }

	IEnumerator C_StopFallDown()
	{
		yield return new WaitForSeconds(0.4f);
        m_PlatformCollision.GetComponent<PlatformEffector2D>().rotationalOffset = 0;
        m_PlatformCollision = null;
    }

	public void StartJump()
	{
		if (m_JumpCount < m_totalJumps)
		{
			if (m_GroundSensor.HasDetectedHit() || m_CoyoteTimeCountDown > 0f)
			{
				m_JumpState = true;
				Jump();
			}
		}
		else
		{
			if (m_JumpBufferCoroutine == null)
			{
				m_JumpBufferCountDown = m_JumpBufferTime;
				m_JumpBufferCoroutine = StartCoroutine(C_JumpBuffer());
			}
			if(m_JumpBufferCoroutine != null && m_JumpBufferCountDown <= 0f)
			{
				
                StopCoroutine(m_JumpBufferCoroutine);
                m_JumpBufferCoroutine = null;
				m_JumpBufferCountDown = m_JumpBufferTime;
                m_JumpBufferCoroutine = StartCoroutine(C_JumpBuffer());
            }
		}
	}

	public void StopJump() 
	{
		m_JumpState = false;
		JumpCancel();
        
	}

	private void CancelCrouch()
	{
        if (m_CrouchCoroutine != null)
        {
            m_SpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
			m_SpriteRenderer.gameObject.transform.localPosition = new Vector3(0, 1, 0);
			m_SpriteRenderer.color = Color.red;
            StopCoroutine(m_CrouchCoroutine);
            m_CrouchCoroutine = null;
			if(m_MoveSpeed == 0f)
			{
                m_MoveSpeed = m_TempMoveSpeed;
            }
            m_MoveSpeed *= m_CrouchMoveSpeedMult;
            if (m_RB.constraints != RigidbodyConstraints2D.FreezeRotation)
            {
                m_RB.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
			m_CrouchCheck = false;
        }
    }

    private void Crouch()
    {
        if (m_CrouchCoroutine == null)
        {
			m_SpriteRenderer.gameObject.transform.localScale = new Vector3(1, 0.75f, 1);
            m_SpriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0.75f, 0);
			m_SpriteRenderer.color = Color.blue;
            m_CrouchCheck = true;
            m_MoveSpeed /= m_CrouchMoveSpeedMult;
            m_TempMoveSpeed = m_MoveSpeed;
            m_CrouchCoroutine = StartCoroutine(C_CrouchCoroutine());
            if (m_CrouchBufferCoroutine != null)
            {
                StopCoroutine(m_CrouchBufferCoroutine);
                m_CrouchBufferCoroutine = null;
            }
        }
    }

    public void StartCrouch()
	{

		if (m_GroundSensor.HasDetectedHit())
		{
			Crouch();
		}
		else
		{
			if (m_CrouchBufferCoroutine == null)
			{
				m_CrouchBufferCountDown = m_CrouchBufferTime;
				m_CrouchBufferCoroutine = StartCoroutine(C_CrouchBuffer());
			}
			if (m_CrouchBufferCoroutine != null && m_JumpBufferCountDown <= 0f)
			{

				StopCoroutine(m_CrouchBufferCoroutine);
				m_CrouchBufferCoroutine = null;
				m_CrouchBufferCountDown = m_CrouchBufferTime;
				m_CrouchBufferCoroutine = StartCoroutine(C_CrouchBuffer());
			}
		}
	}

	public void StopCrouch()
	{
		CancelCrouch();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_GroundSensor.HasDetectedHit())
        {
            m_JumpCount = 0;
			if(m_FallThroughCoroutine != null && m_PlatformCollision != null)
			{
				m_PlatformCollision.gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = 0f;
				StopCoroutine (m_FallThroughCoroutine);
				m_FallThroughCoroutine = null;
			}

            if (m_CoyoteCoroutine != null)
            {
                StopCoroutine(m_CoyoteCoroutine);
                m_CoyoteCoroutine = null;
            }
            if (m_RB.linearVelocityX < m_MoveSpeed) { m_RB.linearVelocityX = 0; }
            if (m_AntiGravCheckCoroutine != null)
            {
                StopCoroutine(m_AntiGravCheckCoroutine);
                m_AntiGravCheckCoroutine = null;
            }
            if (m_FallingCoroutine != null)
            {
                StopCoroutine(m_FallingCoroutine);
                m_FallingCoroutine = null;
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
                m_CoyoteCoroutine = StartCoroutine(C_CoyoteTimeReduction());
            }
        }

    }

    IEnumerator C_CrouchBuffer()
	{
        while (m_CrouchBufferCountDown > 0f)
        {
            m_CrouchBufferCountDown -= Time.deltaTime;
            if (m_GroundSensor.HasDetectedHit())
            {
				m_RB.linearVelocityY = 0f;
                Crouch();
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

	IEnumerator C_CrouchCoroutine()
	{
		while (m_CrouchCheck)
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector3(gameObject.transform.position.x + (m_CrouchLimit * m_MoveDir), gameObject.transform.position.y, gameObject.transform.position.z), new Vector2((1f * m_MoveDir), -1), m_CrouchLedgeCheckDistance, m_GroundSensor.GetLayerMask());
			if (!hit)
			{
				m_RB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
				m_MoveSpeed = 0f;
			}
			else
			{
				m_RB.constraints = RigidbodyConstraints2D.FreezeRotation;
				m_MoveSpeed = m_TempMoveSpeed;
			}
			yield return null;
		}
		
	}

	IEnumerator C_JumpBuffer()
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

	IEnumerator C_AntiGravApex()
	{
		yield return new WaitForSeconds(0.1f);
		while(!m_GroundSensor.HasDetectedHit())
		{
			if(m_RB.linearVelocityY > -1f && m_RB.linearVelocityY < 1f)
			{
				while(m_AntiGravCountDown > 0f)
				{
					m_RB.linearVelocityY = 0;
					m_AntiGravCountDown -= Time.deltaTime;
					yield return null;
				}
                if (m_FallingCoroutine == null)
                {
                    m_FallingCoroutine = StartCoroutine(C_FallingCoroutine());
                }
                yield break;
			}
			yield return null;
		}
	}
	IEnumerator C_FallingCoroutine()
	{
        while (!m_GroundSensor.HasDetectedHit())
		{
			if(m_RB.linearVelocityY < -m_FallSpeedCap)
			{
				m_RB.linearVelocityY = -m_FallSpeedCap;
			}
            yield return null;
        }
		
	}

    IEnumerator C_CoyoteTimeReduction()
    {
        while (m_CoyoteTimeCountDown > 0f)
        {
            m_CoyoteTimeCountDown -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    private void OnDrawGizmos()
    {
		Debug.DrawRay(new Vector3(gameObject.transform.position.x+(m_CrouchLimit*m_MoveDir), gameObject.transform.position.y, gameObject.transform.position.z), new Vector2(1f * m_MoveDir, -1), Color.cyan);
    }
}
