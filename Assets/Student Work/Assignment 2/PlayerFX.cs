using System.Collections;
using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class PlayerFX : MonoBehaviour
{
    CharacterMovement m_CharMov;
    Camera m_camera;
    HealthComponent m_PlayerHealth;
    ParticleSystem m_PlayerParticles;
    AudioSource m_Audio;
    [SerializeField] AudioClip m_JumpSFX;
    [SerializeField] AudioClip m_LandSFX;
    [SerializeField] AudioClip m_HitSFX;

    [SerializeField] float m_ShakeDuration = 0f;
    float m_ShakeTimer = 0f;
    [SerializeField] float m_ShakeStrength = 0.7f;
    [SerializeField] float m_ShakeDamping = 1f;
    Vector2 m_initialPos;
    Coroutine m_ShakeCoroutine;

    bool m_landing;

    private void Start()
    {
        m_CharMov = GetComponentInParent<CharacterMovement>();
        m_camera = GetComponentInParent<Camera>();
        m_PlayerHealth = GetComponentInParent<HealthComponent>();
        m_PlayerParticles = GetComponent<ParticleSystem>();
        m_Audio = GetComponent<AudioSource>();

        CharacterMovement.m_cmEvent.OnJump += Jump;
     
        CharacterMovement.m_cmEvent.OnLand += Land;

        CharacterMovement.m_cmEvent.OnHit += Hit;

        CharacterMovement.m_cmEvent.OnFall += Fall;
    }

    void Jump()
    {
        FXPoof();
        PlaySound(m_JumpSFX);
        m_landing = false;
    }

    void Land()
    {
        if (!m_landing)
        {
            FXPoof();
            ScreenShake();
            PlaySound(m_LandSFX);
            m_landing = true;
        }
    }

    void Hit()
    {
        ScreenShake();
        PlaySound(m_HitSFX);
    }

    void Fall()
    {
        m_landing = false;
    }

    private void FXPoof()
    {
        m_PlayerParticles.Play();
    }

    private void ScreenShake()
    {
        Vector2 shakeVector = m_CharMov.GetScreenShakeStrength();
        Debug.Log(shakeVector.magnitude);
        if (shakeVector.magnitude >= 10)
        {
            if (m_ShakeCoroutine == null)
            {
                m_ShakeCoroutine = StartCoroutine(C_Screenshake(shakeVector));
            }
            else
            {
                StopCoroutine(m_ShakeCoroutine);
                m_ShakeCoroutine = null;
                m_ShakeCoroutine = StartCoroutine(C_Screenshake(shakeVector));
            }
        }
        
    }

    private void PlaySound(AudioClip sfx)
    {
        m_Audio.PlayOneShot(sfx);
    }

    IEnumerator C_Screenshake(Vector2 shakeDirection)
    {
        m_initialPos = Camera.main.transform.localPosition;
        m_ShakeTimer = m_ShakeDuration;
        while(m_ShakeTimer > 0)
        {
            Camera.main.transform.localPosition = m_initialPos + ((shakeDirection/200) * Random.Range(0f, 2f) * m_ShakeStrength);
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, -10f);

            m_ShakeTimer -= Time.deltaTime * m_ShakeDamping;
            yield return null;
        }
        m_ShakeTimer = 0;
        Camera.main.transform.localPosition = m_initialPos;
        Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, -10f);

    }


}
