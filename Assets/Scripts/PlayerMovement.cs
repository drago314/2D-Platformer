using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    private void Awake() {

        //Grab references 
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        float horizontalInput = Input.GetAxis("Horizontal");

        //Moving Character Left/Right
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Flipping Character Model
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector2((float) 0.75, (float) 0.75);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2((float) -0.75, (float) 0.75);

        //Jumping
        if(Input.GetKey(KeyCode.Space) && grounded)
            Jump();

        //Set animation parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", grounded);
    }   

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        grounded = false;
        anim.SetTrigger("Jump");
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Ground")
            grounded = true;
    }
}
