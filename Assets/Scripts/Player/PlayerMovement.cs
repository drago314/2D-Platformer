using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float slidingGravity;
    [SerializeField] private float minWallSlideSpeed;
    [SerializeField] private float maxWallSlideSpeed;
    [SerializeField] private float wallJumpTime;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSideForce;
    [SerializeField] private float controlDashTime;
    [SerializeField] private float controlDashSideSpeed;
    [SerializeField] private float controlDashUpSpeed;
    [SerializeField] private float controlDashSlowdown;
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;
    private Vector2 joystickInput;
    private float horizontalInput;
    private bool jumpPressed;
    private bool jumpInputUsed;
    private bool dashPressed;
    private bool dashInputUsed;
    private bool controlDashPressed;
    private float gravityStore;
    private bool isGrounded;
    private bool onLeftWall, onRightWall;
    private bool isSliding, wasSliding;
    private int jumpCounter;
    private float wallJumpTimer;
    private bool canDash;
    private float dashTimer;
    private bool canControlDash;
    private bool isControlDashing;
    private bool wasControlDashing;
    private float controlDashTimer;

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
        //Set animation parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded);

        CheckIfGrounded();
        CheckIfOnWall();
        CheckIfCanJump();
        CheckIfCanDash();
        CheckIfCanControlDash();

        if (wallJumpTimer <= 0 && dashTimer <= 0 && !isControlDashing)
        {
            anim.SetBool("IsDashing", false);

            //Flipping Character Model
            if (horizontalInput > 0.01f)
                transform.localScale = new Vector2(1, 1);
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector2(-1, 1);

            Move();
            Jump();
            WallMovement();
            Dash();
            ControlDash();
        }
        else if (wallJumpTimer >= 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        else if (dashTimer >= 0)
        {
            dashTimer -= Time.deltaTime;
            if (onLeftWall || onRightWall)
            {
                dashTimer = 0;
                anim.SetBool("IsDashing", false);
            }
        }
        else if (isControlDashing)
        {
            controlDashTimer -= Time.deltaTime;
            ControlDash();
        }
    }

    private void Move()
    {
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);
    }

    private void Jump()
    {
        if (jumpCounter < 2 && !jumpInputUsed && !isSliding)
        {
            Invoke("AddJump", 0.1f);
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
            jumpInputUsed = true;
        }
        if (body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.velocity.y > 0 && !jumpPressed)
        {
            body.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void WallMovement()
    {
        if (wasSliding = false && isSliding)
        {
            body.velocity = Vector2.zero;
        }

        if (isSliding == true)
        {
            dashInputUsed = true;
            wasSliding = true;
            body.gravityScale = slidingGravity;
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -maxWallSlideSpeed, -minWallSlideSpeed));
            if (!jumpInputUsed)
            {
                wallJumpTimer = wallJumpTime;
                body.velocity = new Vector2(-horizontalInput * wallJumpSideForce, wallJumpUpForce);
                body.gravityScale = gravityStore;
                isSliding = false;
                jumpInputUsed = true;
            }
        }
        else
        {
            wasSliding = false;
            body.gravityScale = gravityStore;
        }
    }

    private void Dash()
    {
        if (canDash == true && !dashInputUsed)
        {
            anim.SetBool("IsDashing", true);
            dashTimer = dashTime;
            body.velocity = new Vector2(transform.localScale.x * dashSideForce, 0);
            body.gravityScale = 0;
            canDash = false;
            dashInputUsed = true;
        }
    }

    private void ControlDash()
    {
        if (canControlDash && controlDashPressed && (!wasControlDashing || controlDashTimer >= 0) && !(isGrounded || onLeftWall || onRightWall))
            isControlDashing = true;
        else
            isControlDashing = false;

        if (isControlDashing)
        {
            body.gravityScale = 0;
            if (!wasControlDashing)
                controlDashTimer = controlDashTime;
            wasControlDashing = true;
            body.velocity = new Vector2(joystickInput.x * controlDashSideSpeed, joystickInput.y * controlDashUpSpeed);
        } 
        else if (wasControlDashing)
        { 
            wasControlDashing = false;
            canControlDash = false;
            body.gravityScale = gravityStore;
            body.velocity = new Vector2(body.velocity.x / controlDashSlowdown, body.velocity.y / controlDashSlowdown);
        }
    }

    private void CheckIfOnWall()
    {
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.right, 0.1f, groundLayer);
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, groundLayer);
        onLeftWall = raycastHitLeft.collider != null;
        onRightWall = raycastHitRight.collider != null;
        isSliding = !isGrounded && ((onLeftWall && horizontalInput < 0) || (onRightWall && horizontalInput > 0));
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        isGrounded = raycastHit.collider != null;
    }

    private void CheckIfCanJump()
    {
        if (isGrounded || isSliding)
        {
            jumpCounter = 0;
        }
        if (wasSliding)
        {
            jumpCounter = 1;
        }
    }

    private void AddJump()
    {
        jumpCounter += 1;
    }

    private void CheckIfCanDash()
    {
        if (isGrounded || wallJumpTimer > 0)
        {
            canDash = true;
        }
        if (wasSliding)
        {
            canDash = true;
        }
        if (isSliding)
        {
            canDash = false;
        }
    }

    private void CheckIfCanControlDash()
    {
        if (isGrounded || isSliding)
            canControlDash = true;
    }

    private void OnMove(InputValue value)
    {
        joystickInput = value.Get<Vector2>();
        horizontalInput = joystickInput.x;
    }

    private void OnJump(InputValue value)
    {
        jumpPressed = value.isPressed;
        if (jumpPressed)
        {
            jumpInputUsed = false;
        }
    }

    private void OnDash(InputValue value)
    {
        dashPressed = value.isPressed;
        if (dashPressed)
        {
            dashInputUsed = false;
        }
    }

    private void OnControlDash(InputValue value)
    {
        controlDashPressed = value.isPressed;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(1200, 10, 100, 100), "Can controldash: " + canControlDash);
        GUI.Label(new Rect(1200, 50, 100, 100), "is doing thing: " + isControlDashing );
    }
}

