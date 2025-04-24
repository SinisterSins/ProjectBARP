using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float detectionRange = 10f;
    private Rigidbody rb;
    private Transform player;

    // projectile enemy settings
    private bool projectileEnemy;
    private GameObject projectile;
    public float projectileSpeed = 5f;

    public float fireCooldown = 3f;
    private float nextFireTime = 0f;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        projectileEnemy = gameObject.CompareTag("ProjectileEnemy");
        if (projectileEnemy)
        {
            projectile = gameObject.transform.GetChild(0).gameObject;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
            if (projectileEnemy && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireCooldown;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
    void Shoot()
    {
        GameObject projectileClone = Instantiate(projectile, transform.position, projectile.transform.rotation);
        projectileClone.GetComponent<Rigidbody>().linearVelocity = new Vector3(-projectileSpeed, 0f, 0f); 
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
