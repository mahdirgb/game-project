using UnityEngine;

public class PingPongMover : MonoBehaviour
{
    [Tooltip("How far left/right the object moves from center (x = 0)")]
    public float targetX = 4f;

    [Tooltip("Speed of movement in units per second")]
    public float speed = 2f;

    private float direction = 1f;

    void Update()
    {
        // Move horizontally
        transform.position += Vector3.right * speed * direction * Time.deltaTime;

        // Flip direction if limits are reached
        if (transform.position.x >= targetX)
        {
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            direction = -1f;
        }
        else if (transform.position.x <= -targetX)
        {
            transform.position = new Vector3(-targetX, transform.position.y, transform.position.z);
            direction = 1f;
        }
    }
}
