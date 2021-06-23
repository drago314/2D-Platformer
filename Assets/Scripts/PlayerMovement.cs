using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float slidingGravity;
    [SerializeField] private float wallJumpTime;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;
    private float horizontalInput;
    private float gravityStore;
    private bool isGrounded;
    private bool onLeftWall, onRightWall;
    private bool isSliding;
    private int jumpCounter;
    private float wallJumpTimer;

    private void Awake()
    {
        //Grab references 
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        gravityStore = body.gravityScale;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Set animation parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded);

        CheckIfGrounded();
        CheckIfOnWall();
        CheckIfCanJump();
        if (wallJumpTimer <= 0)
        {
            //Flipping Character Model
            if (horizontalInput > 0.01f)
                transform.localScale = new Vector2(1, 1);
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector2(-1, 1);

            Move();
            Jump();
            WallMovement();
        }
        else
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void Move()
    {
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter < 2)
        {
            Invoke("AddJump", 0.1f);
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }
        if (body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            body.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void WallMovement()
    {
        isSliding = !isGrounded && ((onLeftWall && horizontalInput < 0) || (onRightWall && horizontalInput > 0));

        if (isSliding == true)
        {
            body.gravityScale = slidingGravity;
            body.velocity = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                wallJumpTimer = wallJumpTime;
                body.velocity = new Vector2(-horizontalInput * wallJumpSideForce, wallJumpUpForce);
                body.gravityScale = gravityStore;
                isSliding = false;
                jumpCounter = 1;
            }
        }
        else
        {
            body.gravityScale = gravityStore;
        }
    }

    private void CheckIfOnWall()
    {
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.right, 0.1f, groundLayer);
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, groundLayer);
        onLeftWall = raycastHitLeft.collider != null;
        onRightWall = raycastHitRight.collider != null;
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        isGrounded = raycastHit.collider != null;
    }

    private void CheckIfCanJump()
    {
        if (isGrounded)
        {
            jumpCounter = 0;
        }
    }

    private void AddJump()
    {
        jumpCounter += 1;
    }

    private void OnGUI()
    {
        if (Application.isEditor)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Jump Counter: " + jumpCounter);
        }
    }
}

