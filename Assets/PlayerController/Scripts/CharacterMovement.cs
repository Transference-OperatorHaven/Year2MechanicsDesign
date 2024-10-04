using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

	[SerializeField] private StatefulRaycastSensor2D m_GroundSensor;
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;

	private float m_InMove;

	//[SerializeField] private float m_JumpBufferTime;
    //[SerializeField] private float m_JumpBufferTimeMark;

    [SerializeField]private float m_CoyoteTime;
	private float m_CoyoteTimeMark;

	private void Awake()
	{
		m_RB = GetComponent<Rigidbody2D>();
		Debug.Assert(m_GroundSensor != null);
	}

	public void SetInMove(float newMove) => m_InMove = newMove;
	public void StartJump()
	{
		if (m_GroundSensor.HasDetectedHit() || m_CoyoteTimeMark >= Time.time)
		{
			m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
		}
	}
	public void StopJump() { }

	private void FixedUpdate()
	{
		m_RB.linearVelocityX = m_MoveSpeed * m_InMove;
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(m_RB.linearVelocityY < 0)
		{
			m_CoyoteTimeMark = m_CoyoteTime + Time.time;
		}
    }
}
