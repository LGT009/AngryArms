using UnityEngine;

public class FollowRigidBody : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() // Use FixedUpdate for physics
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 movement = direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
            // Optionally use rb.MoveRotation to orient towards target
        }
    }
}
