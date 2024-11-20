using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColourShooterBase : MonoBehaviour
{
    [SerializeField] protected GameObject m_ShootingStart;
    public enum ColoursToShoot
    {
        neutral,
        orange,
        blue,
        all,
        norange,
        bleutral,
        blorange
    }
    [SerializeField] protected ColoursToShoot m_ColoursToShoot;
    [Header("Shooting Settings")]
    [SerializeField] protected GameObject m_Projectile;
    [SerializeField] protected float m_ProjectileSpeed;
    [SerializeField] protected int m_NumOfShotsPerColour;
    protected List<Color> m_ShooterColours = new List<Color>();
    [SerializeField] protected LayerMask m_NeutralCollision;
    [SerializeField] protected LayerMask m_OrangeCollision;
    [SerializeField] protected LayerMask m_BlueCollision;
    protected List<LayerMask> m_ShooterLayers = new List<LayerMask>();
    [SerializeField] protected float m_ShootingDelay;
    [SerializeField] protected float m_ColourShiftDelay;
    protected bool m_IsFiring;
    protected Coroutine m_ShootingCoroutine;
    protected SpriteRenderer m_Renderer;

    protected int GetLayerFromMask(LayerMask layerMask)
    {
        float layerNumberfloat = Mathf.Log(layerMask.value, 2);
        return Mathf.FloorToInt(layerNumberfloat);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CompileColours();
            StartFiring();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopFiring();
        }
    }

    protected void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        CompileColours();
    }

    protected void CompileColours()
    {
        switch (m_ColoursToShoot)
        {
            case ColoursToShoot.neutral:
                m_ShooterColours.Add(Color.white);
                m_ShooterLayers.Add(m_NeutralCollision);  break;
            case ColoursToShoot.orange:
                m_ShooterColours.Add(new Vector4(1, 0.5f, 0, 1));
                m_ShooterLayers.Add(m_OrangeCollision); break;
            case ColoursToShoot.blue:
                m_ShooterColours.Add(Color.blue);
                m_ShooterLayers.Add(m_BlueCollision); break;
            case ColoursToShoot.all:
                m_ShooterColours.Add(Color.white); m_ShooterColours.Add(new Vector4(1, 0.5f, 0, 1)); m_ShooterColours.Add(Color.blue);
                m_ShooterLayers.Add(m_NeutralCollision); m_ShooterLayers.Add(m_OrangeCollision); m_ShooterLayers.Add(m_BlueCollision); break;
            case ColoursToShoot.norange:
                m_ShooterColours.Add(Color.white); m_ShooterColours.Add(new Vector4(1, 0.5f, 0, 1));
                m_ShooterLayers.Add(m_NeutralCollision); m_ShooterLayers.Add(m_OrangeCollision); break;
            case ColoursToShoot.bleutral:
                m_ShooterColours.Add(Color.white); m_ShooterColours.Add(Color.blue);
                m_ShooterLayers.Add(m_NeutralCollision); m_ShooterLayers.Add(m_BlueCollision); break;
            case ColoursToShoot.blorange:
                m_ShooterColours.Add(new Vector4(1, 0.5f, 0, 1)); m_ShooterColours.Add(Color.blue);
                m_ShooterLayers.Add(m_OrangeCollision); m_ShooterLayers.Add(m_BlueCollision); break;
        }
    }

    protected virtual void StartFiring()
    {
        if(m_ShootingCoroutine == null)
        {
            m_IsFiring = true;
            m_ShootingCoroutine = StartCoroutine(C_ShootCoroutine());
        }
    }

    protected void StopFiring()
    {
        if(m_ShootingCoroutine != null)
        {
            m_IsFiring = false;
            StopCoroutine(m_ShootingCoroutine);
            m_ShootingCoroutine = null;
        }
    }

    protected virtual void SetShooter(int i)
    {
        m_Renderer.color = m_ShooterColours[i];
        gameObject.layer = GetLayerFromMask(m_ShooterLayers[i]);
    }

    protected void MakeProj(int i)
    {
        GameObject proj = Instantiate(m_Projectile, m_ShootingStart.transform.position, Quaternion.identity);
        proj.GetComponentInChildren<SpriteRenderer>().color = m_ShooterColours[i];
        proj.layer = GetLayerFromMask(m_ShooterLayers[i]);
        proj.GetComponentInChildren<Rigidbody2D>().linearVelocity = (m_ShootingStart.transform.position - transform.position) * m_ProjectileSpeed;
    }

    protected virtual IEnumerator C_ShootCoroutine()
    {
        while (m_IsFiring)
        {
            for (int i = 0; i < m_ShooterColours.Count; i++)
            {
                SetShooter(i);
                yield return new WaitForSeconds(m_ShootingDelay / 2);
                for (int j = 0; j < m_NumOfShotsPerColour; j++)
                {
                    MakeProj(i);
                    yield return new WaitForSeconds(m_ShootingDelay);
                }
                yield return new WaitForSeconds(m_ColourShiftDelay);
            }

        }

    }
}
