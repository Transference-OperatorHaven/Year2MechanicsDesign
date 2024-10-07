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
	[SerializeField] private float m_mostRecentContactY;


	//[SerializeField] private float m_JumpBufferTime;
	//[SerializeField] private float m_JumpBufferTimeMark;

	[SerializeField] private float m_CoyoteTime;
	private float m_CoyoteTimeMark;

	private void Awake()
	{
		m_RB = GetComponent<Rigidbody2D>();
		Debug.Assert(m_GroundSensor != null);
	}

	public void SetInMove(float newMove) => m_RB.linearVelocityX = m_MoveSpeed * newMove;

	public void StartJump()
	{
		if (m_JumpCount < m_totalJumps)
		{
			if (m_GroundSensor.HasDetectedHit() || m_CoyoteTimeMark >= Time.time)
			{
				m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
				m_JumpCount++;
			}
		}
	}

	public void StopJump() { }

	private void FixedUpdate()
	{
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		m_mostRecentContactY = collision.GetContact(0).point.y;
		if(m_GroundSensor.HasDetectedHit())
		{
			m_JumpCount = 0;
		}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
		//Debug.Log(m_mostRecentContactY + " + " + gameObject.transform.position.y);

		if (m_mostRecentContactY <= gameObject.transform.position.y)
		{
            if (m_RB.linearVelocityY < 0)
            {
				Debug.Log("setting coyote time");
                m_CoyoteTimeMark = m_CoyoteTime + Time.time;
            }
        }
        
    }
}
