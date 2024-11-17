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

    private void Awake()
    {
        m_SpriteRenderer = m_Player.GetComponentInChildren<SpriteRenderer>();
        m_CapsuleCollider = m_Player.GetComponentInChildren<CapsuleCollider2D>();
        ChangeColour();

    }

    private void Update()
    {
    }

    public void ChangeColour()
    {
        if(m_IsOrange)
        {
            float layerNumberfloat = Mathf.Log(m_BlueLayer.value, 2);
            int layerNumber = Mathf.FloorToInt(layerNumberfloat);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 2, m_OrangeCollision);
            if(hit) { return; }
            m_IsOrange = false;
            m_Player.layer = layerNumber;
            m_CapsuleCollider.gameObject.layer = layerNumber;
            EmitOrb();
            m_SpriteRenderer.color = Color.blue;
        }
        else
        {
            float layerNumberfloat = Mathf.Log(m_OrangeLayer.value, 2);
            int layerNumber = Mathf.FloorToInt(layerNumberfloat);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 2, m_BlueCollision);
            if (hit) { return; }
            m_IsOrange = true;
            m_Player.layer = layerNumber;
            m_CapsuleCollider.gameObject.gameObject.layer = layerNumber;
            EmitOrb();
            m_SpriteRenderer.color = new Vector4(1, 0.5f, 0, 1);
        }
    }

    void EmitOrb()
    {
        if (m_IsOrange)
        {

        }
        else
        {

        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.up * 2), Color.red);
    }
}
