using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float detectionRange = 10f;
    private Rigidbody rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 collisionDirection = collision.contacts[0].normal;

            if (collisionDirection.y < -0.5f)
            {
                // die if the player lands on the head
                Die();
                collision.gameObject.GetComponent<PlayerController>()?.Bounce();
            }
            else
            {
                // enemy touches player from sides or bottom, player dies
                collision.gameObject.GetComponent<PlayerController>()?.Die();
            }
        }
    }

    public void Die()
    {
        Debug.Log(name + " died!");

        // destroys the object for now
        Destroy(gameObject);
    }
}
