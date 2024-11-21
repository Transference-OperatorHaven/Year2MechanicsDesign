using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 Velocity = Vector3.zero;

    [SerializeField] private Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref Velocity, smoothTime);
    }
}
