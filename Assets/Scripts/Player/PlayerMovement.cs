using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7.5f;
    [SerializeField] private float moveSmoothing = 0.075f;
    [SerializeField] private float fallDistanceMultiplier = 1;
    [SerializeField] private float fallAccelerationMultiplier = 1;
    [SerializeField] private int maxJumpsAllowed = 2;
    [SerializeField] private float jumpForce = 150f;

    public bool isGrounded;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 currentVelocity;
    private Vector2 targetVelocity;
    private bool isFacingRight;
    private int jumpCount;
    private float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ResetJump();
    }

    private void Update()
    {
        //Calculates player movement based on input
        CheckForMovement();
        //Checks for player jump input
        if (Input.GetKeyDown(PlayerPrefData.Jump)) Jump();
    }

    private void CheckForMovement()
    {
        horizontalInput = 0;
        if (Input.GetKey(PlayerPrefData.Left)) horizontalInput -= 1;
        if (Input.GetKey(PlayerPrefData.Right)) horizontalInput += 1;

        currentVelocity = rb.velocity;
        if (animator != null) animator.SetFloat("velocity", currentVelocity.x);

        //Checks to see if the character is currently falling and makes them fall a little faster
        if (rb.velocity.y < 0) targetVelocity = new Vector2(horizontalInput * movementSpeed * fallDistanceMultiplier, currentVelocity.y * fallAccelerationMultiplier);
        else targetVelocity = new Vector2(horizontalInput * movementSpeed, currentVelocity.y);

        //Checks to see if there is currently any horizontal input and if not, stops player horizontal movement
        if (horizontalInput != 0) rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, moveSmoothing);
        else rb.velocity = new Vector2(0, rb.velocity.y);

        //Flips the character sprite direction
        if (horizontalInput > 0 && !isFacingRight) Flip();
        else if (horizontalInput < 0 && isFacingRight) Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
    }

    private void Jump()
    {
        if (jumpCount > 0) //Makes sure there is still another jump available
        {
            jumpCount--;
            isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, 0); //Zeros out the velocity on the y axis to make a bouncy second jump
            rb.AddForce(Vector2.up * jumpForce * 100); //Adds upward force
        }
    }

    private void ResetJump()
    {
        isGrounded = true; //Flags the player as on the ground
        jumpCount = maxJumpsAllowed; //Resets the double jump
        if (animator != null) animator.SetBool("IsJumping", false); //Makes sure the jumping animation is no longer playing
        //playerSound.PlayLandingSound(); //Plays a landing sound
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks box collider trigger to see if the player is grounded or not
        if (collision.gameObject.layer == 10)
            ResetJump();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Checks box collider trigger to see if the player is grounded or not
        if (collision.gameObject.layer == 10)
            isGrounded = false;
    }
}
