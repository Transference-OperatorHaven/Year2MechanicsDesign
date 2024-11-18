using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColourShooterBase : MonoBehaviour
{
    [SerializeField] Transform m_ShootingStart;
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
    [SerializeField] ColoursToShoot m_ColoursToShoot;
    [Header("Shooting Settings")]
    [SerializeField] GameObject m_Projectile;
    [SerializeField] float m_ProjectileSpeed;
    [SerializeField] private int m_NumOfShotsPerColour;
    List<Color> m_ShooterColours = new List<Color>();
    [SerializeField] private LayerMask m_NeutralCollision;
    [SerializeField] private LayerMask m_OrangeCollision;
    [SerializeField] private LayerMask m_BlueCollision;
    List<LayerMask> m_ShooterLayers = new List<LayerMask>();
    [SerializeField] private float m_ShootingDelay;
    [SerializeField] private float m_ColourShiftDelay;
    private bool m_IsFiring;
    private Coroutine m_ShootingCoroutine;
    SpriteRenderer m_Renderer;

    int GetLayerFromMask(LayerMask layerMask)
    {
        float layerNumberfloat = Mathf.Log(layerMask.value, 2);
        return Mathf.FloorToInt(layerNumberfloat);
    }

    private void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        CompileColours();
        StartFiring();
    }

    void CompileColours()
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

    void StartFiring()
    {
        if(m_ShootingCoroutine == null)
        {
            m_IsFiring = true;
            m_ShootingCoroutine = StartCoroutine(C_ShootCoroutine());
        }
    }

    void StopFiring()
    {
        if(m_ShootingCoroutine != null)
        {
            m_IsFiring = false;
            StopCoroutine(m_ShootingCoroutine);
            m_ShootingCoroutine = null;
        }
    }

    IEnumerator C_ShootCoroutine()
    {
        while (m_IsFiring)
        {
            for (int i = 0; i < m_ShooterColours.Count; i++)
            {
                m_Renderer.color = m_ShooterColours[i];
                gameObject.layer = GetLayerFromMask(m_ShooterLayers[i]);
                yield return new WaitForSeconds(m_ShootingDelay / 2);
                for (int j = 0; j < m_NumOfShotsPerColour; j++)
                {
                    GameObject proj = Instantiate(m_Projectile, m_ShootingStart.transform.position, Quaternion.identity);
                    proj.GetComponentInChildren<SpriteRenderer>().color = m_ShooterColours[i];
                    proj.layer = GetLayerFromMask(m_ShooterLayers[i]);
                    proj.GetComponentInChildren<Rigidbody2D>().linearVelocity = (m_ShootingStart.transform.position - transform.position) * m_ProjectileSpeed;
                    yield return new WaitForSeconds(m_ShootingDelay);
                }
                yield return new WaitForSeconds(m_ColourShiftDelay);
            }

        }

    }
}
