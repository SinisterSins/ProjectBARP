using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // movement settings
    public float runSpeed = 7f;
    public float jumpForce = 10f;
    public float slideDuration = 1f;

    // movement logic settings
    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isSliding = false;
    private float slideTimer;

    // player orientation constants
    private Quaternion uprightRotation;
    private Quaternion slideRotation;
    private Vector3 originalPosition;

    // jump and fall physics
    public float fallMultiplier = 2.5f; 
    public float lowJumpMultiplier = 2f;

    // detection variables
    private float groundRayLength;
    private float wallCheckDistance;
    public LayerMask groundLayer;

    // combat settings
    public Transform punchPoint; // empty GameObject placed slightly in front of player
    public float punchRadius = 1.0f;
    public LayerMask enemyLayer;

    void Start()
    {
        // define rigidBody and orientation constants
        rb = GetComponent<Rigidbody>();
        uprightRotation = Quaternion.Euler(0f, 0f, 0f);
        slideRotation = Quaternion.Euler(0f, 0f, 90f);
        transform.rotation = uprightRotation;

        // lock the players rotation to prevent unnecessary rotation
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // defines distances for collision detection raycasts
        Collider col = GetComponent<Collider>();
        groundRayLength = col.bounds.size.y / 2f;
        wallCheckDistance = col.bounds.size.x / 2f;
        originalPosition = transform.position;
    }

    void Update()
    {
        // raycasting for ground detection
        Vector3 rightRayOrigin = transform.position + Vector3.right * wallCheckDistance;
        Vector3 groundRayOrigin = transform.position;
        Vector3 leftRayOrigin = transform.position + Vector3.left * wallCheckDistance;
        // boolean: true if on ground, false if in air
        isGrounded = Physics.Raycast(rightRayOrigin, Vector3.down, groundRayLength, groundLayer) ||
                     Physics.Raycast(groundRayOrigin, Vector3.down, groundRayLength, groundLayer) ||
                     Physics.Raycast(leftRayOrigin, Vector3.down, groundRayLength, groundLayer);

        // raycasting for wall detection
        Vector3 topRayOrigin = transform.position + Vector3.up * groundRayLength;
        Vector3 centerRayOrigin = transform.position;
        Vector3 bottomRayOrigin = transform.position + Vector3.down * groundRayLength;
        // boolean: true if obstructed by a wall, false if unobstructed
        bool wallAhead =
            Physics.Raycast(topRayOrigin, Vector3.right, wallCheckDistance, groundLayer) ||
            Physics.Raycast(centerRayOrigin, Vector3.right, wallCheckDistance, groundLayer) ||
            Physics.Raycast(bottomRayOrigin, Vector3.right, wallCheckDistance, groundLayer);

        // if player is obstructed, stop applying forward velocity
        if (wallAhead)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y - 5f * Time.deltaTime, 0f);
        }
        else
        {
            rb.linearVelocity = new Vector3(runSpeed, rb.linearVelocity.y, 0f);
        }

        // jumper
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            Jump();
        }

        // slider
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && isGrounded && !isSliding)
        {
            StartSlide();
        }

        // ends the slide after alotted (slideTimer) time
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                EndSlide();
            }
        }

        // gravity adjuster: if player is falling down, increase fall velocity to prevent `moon` gravity
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // low jump adjuster: if player taps W/up arrow key vs. holds it down, jumps less high 
        if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // auto resetter if player falls out of world -- will be deleted upon completion of player death
        if (transform.position.y <= -100)
        {
            ResetPlayer();
        }

        // punch
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Punch();
        }
    }

    void Jump()
    {
        // if the player is sliding and decides to jump, end the slide and then jump
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
        Debug.Log("punched");

        // returns an array of Collider enemyObjects that are inside the player`s punch hitbox
        Collider[] hitEnemies = Physics.OverlapSphere(punchPoint.position, punchRadius, enemyLayer);
        // iterates thru each Collider enemyObject and kills them
        
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<EnemyController>()?.Die();
        }
    }

    public void Die()
    {
        Debug.Log("I died :(");
        ResetPlayer();
    }

    public void Bounce()
    {
        rb.linearVelocity =  new Vector3(rb.linearVelocity.x, 0f, 0f);
        rb.AddForce(Vector3.up * jumpForce * 0.75f, ForceMode.Impulse);
    }
}
