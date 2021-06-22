using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier; 
    [SerializeField] private float wallSlidingSpeed;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private bool onWall;
    private float horizontalInput;


    private void Awake()
    {

        //Grab references 
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flipping Character Model
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector2(1, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2(-1, 1);

        //Set animation parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", grounded);

        Move();
        CheckIfGrounded();
        CheckIfOnWall();
        Jump();
        BetterJump();
        WallSlide();
    }

    private void Move()
    {
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }
    }

    void BetterJump()
    {
        if (body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            body.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void WallSlide()
    {
        if (onWall && !grounded && horizontalInput != 0)
        {
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, -float.MaxValue));
        }
    }
    private void CheckIfOnWall()
    {
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.right, 0.1f, groundLayer);
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, groundLayer);
        onWall = raycastHitLeft.collider != null || raycastHitRight.collider != null;
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        grounded = raycastHit.collider != null;
    }
}

