using UnityEngine;

[RequireComponent (typeof(ParticleSystem))]
[RequireComponent (typeof(AudioSource))]
public class SetRespawnPos : MonoBehaviour
{
    private ParticleSystem m_particle;
    [SerializeField] private AudioSource m_audio;
    [SerializeField] private AudioClip m_checkpointSFX;

    private void Awake()
    {
        m_particle = GetComponent<ParticleSystem>();
        m_audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponentInParent<CharacterMovement>() != null)
        {
            if (collision.gameObject.GetComponentInParent<CharacterMovement>().m_RespawnPos != transform)
            {
                m_particle.Stop();
                m_particle.Play();
                if(m_checkpointSFX != null) { m_audio.PlayOneShot(m_checkpointSFX); }
                collision.gameObject.GetComponentInParent<CharacterMovement>().m_RespawnPos = transform;
                collision.gameObject.GetComponentInParent<HealthComponent>().SetHealth(100);
            }
        }
    }
}
