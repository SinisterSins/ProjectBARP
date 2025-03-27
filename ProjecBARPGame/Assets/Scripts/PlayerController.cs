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
    public LayerMask groundLayer;                  // filter out only platform ground

    private Vector3 originalPosition;




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        uprightRotation = Quaternion.Euler(0f, 0f, 0f);
        slideRotation = Quaternion.Euler(0f, 0f, 90f);
        transform.rotation = uprightRotation;

        // lock rotation
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

        
        //isGrounded = Physics.Raycast(transform.position, Vector3.down, groundRayLength, groundLayer);
        



        Vector3 topRayOrigin = transform.position + Vector3.up * groundRayLength; //using groundRayLength because it stores half of the height value
        Vector3 centerRayOrigin = transform.position;
        Vector3 bottomRayOrigin = transform.position + Vector3.down * groundRayLength;
        
        //Debug.Log("T: " + topRayOrigin + " ||| C: " + centerRayOrigin + " ||| B: " + bottomRayOrigin);
        
        bool wallAhead =
            Physics.Raycast(topRayOrigin, Vector3.right, wallCheckDistance, groundLayer) ||
            Physics.Raycast(centerRayOrigin, Vector3.right, wallCheckDistance, groundLayer) ||
            Physics.Raycast(bottomRayOrigin, Vector3.right, wallCheckDistance, groundLayer);

        Debug.Log("Grounded? " + isGrounded +" ||| Wall? " + wallAhead + " ||| Speed: " + rb.linearVelocity.x + " ||| VSpeed: " + rb.linearVelocity.y); 
        
        // constant forward movement
        if (wallAhead)
            {
                // stop forward motion
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y - 5f * Time.deltaTime, 0f);
            }
        else
            {
                // normal forward autorun
                rb.linearVelocity = new Vector3(runSpeed, rb.linearVelocity.y, 0f);
            }

        //Debug.Log("Grounded? " + isGrounded + "; Wall? " + wallAhead + "; " + rb.linearVelocity.x);

        // Jump with W or Up Arrow
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            Jump();
        }

        // Slide with S or Down Arrow
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && isGrounded && !isSliding)
        {
            StartSlide();
        }

        // Countdown slide duration
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

        // if the player taps jump instead of holding it, it will jump less high
        if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (transform.position.y <= -100)
        {
            ResetPlayer();
        }


    }

    void Jump()
    {
        if (isSliding)
        {
            EndSlide();
        }

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


   
}


