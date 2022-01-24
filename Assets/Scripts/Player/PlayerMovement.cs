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

    private Transform playerTrans;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 currentVelocity;
    private Vector2 targetVelocity;
    private bool isFacingRight;
    private int jumpCount;
    private float horizontalInput;
    private float startingScale;

    private void Awake()
    {
        playerTrans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start() => startingScale = playerTrans.localScale.x;

    private void Update()
    {
        if (!GameManager.Instance.IsFrozen)
        {
            //Calculates player movement based on input
            CheckForMovement();
            //Checks for player jump input
            if (Input.GetKeyDown(PlayerPrefData.Jump)) Jump();
        }
    }

    private void CheckForMovement()
    {
        //Checks for input from player
        horizontalInput = 0;
        if (Input.GetKey(PlayerPrefData.Left)) horizontalInput -= 1;
        if (Input.GetKey(PlayerPrefData.Right)) horizontalInput += 1;

        //Gets the current velocity and sets the appropriate animation values
        currentVelocity = rb.velocity;
        if (animator != null) animator.SetFloat("horizontalVelocity", Mathf.Abs(currentVelocity.x));
        if (animator != null) animator.SetFloat("verticalVelocity", currentVelocity.y);

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
        if (isFacingRight) playerTrans.localScale = new Vector3(startingScale, startingScale, 1);
        else playerTrans.localScale = new Vector3(-startingScale, startingScale, 1);
    }

    private void Jump()
    {
        if (jumpCount > 0) //Makes sure there is still another jump available
        {
            jumpCount--;
            isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, 0); //Zeros out the velocity on the y axis to make a bouncy second jump
            rb.AddForce(Vector2.up * jumpForce * 100); //Adds upward force
            //if (animator != null) animator.SetBool("isInAir", true); //Plays the jumping animation
        }
    }

    private void ResetJump()
    {
        isGrounded = true; //Flags the player as on the ground
        jumpCount = maxJumpsAllowed; //Resets the double jump
        //playerSound.PlayLandingSound(); //Plays a landing sound
        //if (animator != null) animator.SetBool("isInAir", false); //Plays the landing animation
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
