using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject m_Player;
    HealthComponent m_HealthComponent;
    ColourGame m_cG;
    Image m_FillColour;
    Slider m_Slider;

    private void OnEnable()
    {
        m_HealthComponent = m_Player.GetComponent<HealthComponent>();
        m_Slider = GetComponent<Slider>();
        m_FillColour = transform.GetChild(1).GetComponentInChildren<Image>();
        m_cG = m_Player.transform.GetChild(4).GetComponentInChildren<ColourGame>();

        m_HealthComponent.onDamaged += UpdateHealth;
        m_cG.OnColourChange += UpdateColour;

        m_Slider.value = 1;
    }

    void UpdateHealth(float p_CurrentHealth, float p_MaxHealth, float p_Change, GameObject attacker)
    {
        m_Slider.value = p_CurrentHealth/p_MaxHealth;
    }

    void UpdateColour(Color color)
    {
        m_FillColour.color = color;
    }
}
