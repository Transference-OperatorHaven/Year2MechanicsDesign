using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent (typeof(SpriteRenderer))]
public class ColourShooterFollow : ColourShooterBase
{
    [Header("Player Follow Specific")]
    [SerializeField] GameObject m_Player;
    [SerializeField] float m_Range;
    SpriteRenderer m_SpriteBarrel;
    Coroutine m_RotateCoroutine;

    private void Awake()
    {
        m_SpriteBarrel = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    protected override void StartFiring()
    {
        base.StartFiring();
        if(m_RotateCoroutine == null)
        {
            m_RotateCoroutine = StartCoroutine(C_RotateCoroutine());
        }
    }

    protected override void SetShooter(int i)
    {
        m_Renderer.color = m_ShooterColours[i];
        m_SpriteBarrel.color = m_ShooterColours[i];
    }

    private Quaternion RotateTowardsTarget()
    {
        float offset = 180f;
        Vector2 direction = (m_Player.transform.position + (Vector3)m_Player.GetComponent<Rigidbody2D>().linearVelocity) - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    void SetShootingPos()
    {
        Vector2 v0 = transform.position;
        Vector2 v1 = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y+1, m_Player.transform.position.z);
        float D1 = (v1 - v0).magnitude;
        Vector2 v2 = (D1 / m_ProjectileSpeed) * m_Player.GetComponent<Rigidbody2D>().linearVelocity;
        Vector2 v3 = v1 + v2;
        float D2 = (v3 - v0).magnitude;
        Vector2 v4 = v3 - v0;
        Vector2 point = v0 + v4 / v4.magnitude * m_Range;
        m_ShootingStart.transform.position = point;

    }

    IEnumerator C_RotateCoroutine()
    {
        while(m_IsFiring)
        {
            m_SpriteBarrel.transform.rotation = RotateTowardsTarget();
            yield return new WaitForFixedUpdate();
        }

        m_RotateCoroutine = null;
    }

    protected override IEnumerator C_ShootCoroutine()
    {
        while (m_IsFiring)
        {
            for (int i = 0; i < m_ShooterColours.Count; i++)
            {
                SetShooter(i);
                yield return new WaitForSeconds(m_ShootingDelay / 2);
                for (int j = 0; j < m_NumOfShotsPerColour; j++)
                {
                    SetShootingPos();
                    MakeProj(i);
                    yield return new WaitForSeconds(m_ShootingDelay);
                }
                yield return new WaitForSeconds(m_ColourShiftDelay);
            }

        }

    }
}
