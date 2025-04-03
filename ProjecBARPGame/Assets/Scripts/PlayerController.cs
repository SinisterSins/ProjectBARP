using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 7f;
    public float jumpForce = 10f;
    public float slideDuration = 1f;

    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isSliding = false;
    private float slideTimer;

    private Quaternion uprightRotation;
    private Quaternion slideRotation;

    public float fallMultiplier = 2.5f; 
    public float lowJumpMultiplier = 2f;

    private float groundRayLength;
    private float wallCheckDistance;
    public LayerMask groundLayer;

    private Vector3 originalPosition;

    
    public Transform punchPoint; // empty GameObject placed slightly in front of player
    public float punchRadius = 1.0f;
    public LayerMask enemyLayers;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        uprightRotation = Quaternion.Euler(0f, 0f, 0f);
        slideRotation = Quaternion.Euler(0f, 0f, 90f);
        transform.rotation = uprightRotation;

        rb.constraints = RigidbodyConstraints.FreezeRotation;

        Collider col = GetComponent<Collider>();
        groundRayLength = col.bounds.size.y / 2f;
        wallCheckDistance = col.bounds.size.x / 2f;
        originalPosition = transform.position;
    }

    void Update()
    {
        Vector3 rightRayOrigin = transform.position + Vector3.right * wallCheckDistance;
        Vector3 groundRayOrigin = transform.position;
        Vector3 leftRayOrigin = transform.position + Vector3.left * wallCheckDistance;

        isGrounded = Physics.Raycast(rightRayOrigin, Vector3.down, groundRayLength, groundLayer) ||
                     Physics.Raycast(groundRayOrigin, Vector3.down, groundRayLength, groundLayer) ||
                     Physics.Raycast(leftRayOrigin, Vector3.down, groundRayLength, groundLayer);

        Vector3 topRayOrigin = transform.position + Vector3.up * groundRayLength;
        Vector3 centerRayOrigin = transform.position;
        Vector3 bottomRayOrigin = transform.position + Vector3.down * groundRayLength;

        bool wallAhead =
            Physics.Raycast(topRayOrigin, Vector3.right, wallCheckDistance, groundLayer) ||
            Physics.Raycast(centerRayOrigin, Vector3.right, wallCheckDistance, groundLayer) ||
            Physics.Raycast(bottomRayOrigin, Vector3.right, wallCheckDistance, groundLayer);

        if (wallAhead)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y - 5f * Time.deltaTime, 0f);
        }
        else
        {
            rb.linearVelocity = new Vector3(runSpeed, rb.linearVelocity.y, 0f);
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            Jump();
        }

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && isGrounded && !isSliding)
        {
            StartSlide();
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                EndSlide();
            }
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (transform.position.y <= -100)
        {
            ResetPlayer();
        }

        
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Punch();
        }
    }

    void Jump()
    {
        if (isSliding)
            EndSlide();

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        transform.rotation = slideRotation;
    }

    void EndSlide()
    {
        isSliding = false;
        transform.rotation = uprightRotation;
    }

    void ResetPlayer()
    {
        transform.position = originalPosition;
    }

    void Punch()
    {
        Debug.Log("Punched!");

        Collider[] hitEnemies = Physics.OverlapSphere(punchPoint.position, punchRadius, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
           // Debug.Log("hit " + enemy.name);
            //enemy.GetComponent<EnemyController>()?.Die();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (punchPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRadius);
    }
}
