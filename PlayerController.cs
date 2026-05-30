using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 20f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.1f;

    [Header("Input ")]
    [SerializeField] private string inputNameHorizontal;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode alternativeJumpKey;

    private Rigidbody2D rb;
    private float inputHorizontal;
    private bool jumpRequested;
    private bool isGrounded;

    public SpawnsData spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw(inputNameHorizontal);

        if ((Input.GetKeyDown(jumpKey) || Input.GetKeyDown(alternativeJumpKey)) && isGrounded)
            jumpRequested = true;
    }

    void FixedUpdate()
    {
        CheckGround();
        Move();
        Jump();
        ApplyFall();
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Move()
    {
        float targetSpeed = inputHorizontal * moveSpeed;

        float rate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, rate * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (!jumpRequested) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpRequested = false;
    }

    private void ApplyFall()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if(rb.linearVelocity.y > 0 && !Input.GetKey(jumpKey))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}
