using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;


public class ColourGame : MonoBehaviour
{
    [SerializeField] private LayerMask m_OrangeCollision;
    [SerializeField] private LayerMask m_BlueCollision;
    [SerializeField] private LayerMask m_OrangeLayer;
    [SerializeField] private LayerMask m_BlueLayer;
    
    private bool m_IsOrange;
    [SerializeField] private GameObject m_Player;
    private SpriteRenderer m_SpriteRenderer;
    CapsuleCollider2D m_CapsuleCollider;
    AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_ColourChangeSFXOrange;
    [SerializeField] private AudioClip m_ColourChangeSFXBlue;

    [Header("Colour Change Cooldown")]
    [SerializeField] private float m_ChangeCooldown;
    [SerializeField] private bool m_CanChange = true;
    private Coroutine m_ChangeCooldownCoroutine;

    [Header("Emit Orb Setting")]
    [SerializeField] private float m_EmitOrbTime;
    private Coroutine m_EmitOrbCoroutine;
    private float m_EmitOrbTimer;
    private SpriteRenderer m_Orb;

    [Header("Colour Change Buffer")]
    [SerializeField] private float m_ChangeBufferTime;
    private Coroutine m_ChangeBufferCoroutine;
    private float m_ChangeBufferTimer;

    #region Events
    public event Action<Color> OnColourChange;
    #endregion

    private void Awake()
    {
        m_SpriteRenderer = m_Player.GetComponentInChildren<SpriteRenderer>();
        m_CapsuleCollider = m_Player.GetComponentInChildren<CapsuleCollider2D>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Orb = GetComponentInChildren<SpriteRenderer>();
        ChangeColourInput();

    }

    private void Update()
    {
    }

    public void ChangeColourInput()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 2, m_IsOrange ? m_OrangeCollision : m_BlueCollision);
        if (!hit)
        {
            ColourChange();
        }
        else
        {
            if (m_ChangeBufferCoroutine == null)
            {
                m_ChangeBufferTimer = m_ChangeBufferTime;
                m_ChangeBufferCoroutine = StartCoroutine(C_ChangeBuffer());
            }
            if (m_ChangeBufferCoroutine != null && m_ChangeBufferTimer <= 0f)
            {

                StopCoroutine(m_ChangeBufferCoroutine);
                m_ChangeBufferCoroutine = null;
                m_ChangeBufferTimer = m_ChangeBufferTime;
                m_ChangeBufferCoroutine = StartCoroutine(C_ChangeBuffer());
            }
        }
    }

    void ColourChange()
    {
        if (!m_CanChange) {  return; }
        if (m_IsOrange)
        {
            float layerNumberfloat = Mathf.Log(m_BlueLayer.value, 2);
            int layerNumber = Mathf.FloorToInt(layerNumberfloat);
            m_IsOrange = false;
            m_Player.layer = layerNumber;
            m_CapsuleCollider.gameObject.layer = layerNumber;
            EmitOrb();
            m_SpriteRenderer.color = Color.blue;
            OnColourChange(Color.blue);
        }
        else
        {
            float layerNumberfloat = Mathf.Log(m_OrangeLayer.value, 2);
            int layerNumber = Mathf.FloorToInt(layerNumberfloat);
            m_IsOrange = true;
            m_Player.layer = layerNumber;
            m_CapsuleCollider.gameObject.gameObject.layer = layerNumber;
            EmitOrb();
            m_SpriteRenderer.color = new Vector4(1, 0.5f, 0, 1);
            OnColourChange?.Invoke(new Vector4(1, 0.5f, 0, 1));
        }

        if(m_ChangeCooldownCoroutine == null)
        {
            m_ChangeCooldownCoroutine = StartCoroutine(C_ChangeCooldown());
        }
    }
   
    void EmitOrb()
    {
        m_EmitOrbTimer = m_EmitOrbTime;
        

        if (m_IsOrange)
        {
            if(m_EmitOrbCoroutine == null)
            {
                m_AudioSource.PlayOneShot(m_ColourChangeSFXOrange);
                m_Orb.color = new Vector4(1, 0.5f, 0, 0.5f);
                m_Orb.enabled = true;
                m_EmitOrbCoroutine = StartCoroutine(C_EmitOrbCoroutine());
            }
            

        }
        else
        {
            if (m_EmitOrbCoroutine == null)
            {
                m_AudioSource.PlayOneShot(m_ColourChangeSFXBlue);
                m_Orb.color = new Vector4(0, 0, 1, 0.5f);
                m_Orb.enabled = true;
                m_EmitOrbCoroutine = StartCoroutine(C_EmitOrbCoroutine());
            }
        }
    }

    IEnumerator C_ChangeCooldown()
    {
        m_CanChange = false;
        yield return new WaitForSeconds(m_ChangeCooldown);
        m_CanChange = true;
        m_ChangeCooldownCoroutine = null;
    }

    IEnumerator C_ChangeBuffer()
    {
        while (m_ChangeBufferTimer > 0f)
        {
            m_ChangeBufferTimer -= Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 2, m_IsOrange ? m_OrangeCollision : m_BlueCollision);
            if (!hit)
            {
                ColourChange();
                m_ChangeBufferCoroutine = null;
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    IEnumerator C_EmitOrbCoroutine()
    {
        List<Collider2D> collidersHit = new List<Collider2D>();
        Rigidbody2D m_RB = m_Player.GetComponent<Rigidbody2D>();
        Vector2 storedMomentum = m_RB.linearVelocity;
        m_RB.constraints = RigidbodyConstraints2D.FreezeAll;
        while (m_EmitOrbTimer > 0)
        {
            float scale = Mathf.Lerp(1f, 3f, 1 - (m_EmitOrbTimer/m_EmitOrbTime));
            RaycastHit2D[] hit = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y + 1), scale*0.75f, Vector2.zero);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.GetComponentInParent<Rigidbody2D>() != null && !collidersHit.Contains(hit[i].collider))
                {
                    Vector2 newDir = (hit[i].collider.gameObject.transform.position - new Vector3(transform.position.x, transform.position.y + 1)) * hit[i].collider.gameObject.GetComponentInParent<Rigidbody2D>().linearVelocity.Abs();
                    hit[i].collider.gameObject.GetComponentInParent<Rigidbody2D>().linearVelocity = newDir;
                    collidersHit.Add(hit[i].collider);
                }
            }
            m_Orb.transform.localScale = new Vector3(scale, scale, scale);
            m_EmitOrbTimer -= Time.deltaTime;
            yield return null;
        }
        m_RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_RB.linearVelocity = storedMomentum;
        if (m_Player.GetComponentInChildren<StatefulRaycastSensor2D>().RunCheck()) { m_RB.linearVelocityX = 0; }
        m_Orb.enabled = false;
        StopCoroutine(m_EmitOrbCoroutine);
        m_EmitOrbCoroutine = null;
    }
}
