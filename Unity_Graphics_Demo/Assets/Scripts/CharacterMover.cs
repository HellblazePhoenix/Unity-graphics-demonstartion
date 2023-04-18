using UnityEngine;

//TODO:
//1. Add RayCast for bouncepad and future interactions as well as an interact key.
//2. Add a max fall speed and clamp in fixed update.
//3. Find sideways moving animations.
//4. find a way to play the forward animation backwards and use that for backwards movement.

public class CharacterMover : MonoBehaviour
{
    //physics ______________________
    public float speed = 10;
    public float jumpHeight = 12;
    public float gravity;
    public float mass = 200;
    public Vector3 velocity;
    Vector2 moveInput = new Vector2();
    public Vector3 hitDirection;

    // Components and objects _______________
    // public GameObject projectilePrefab;
    CharacterController cc;
    Animator animator;
    Transform cam;

    // bools ______________________
    public bool jumpInput;
    public bool isGrounded = true;
    

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
        gravity = -Physics.gravity.y;
    }

    void Update()
    {
        // you can't jump while moving left + up because key jamming ignores the spacebar.
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        jumpInput = Input.GetButton("Jump");

        animator.SetFloat("Forwards", moveInput.y);
        animator.SetBool("Jump", !isGrounded);
    }

    void FixedUpdate()
    {
        Vector3 delta;

        // player movement using WASD or arrow keys

        // find the horizontal unit vector facing forward from the camera
        Vector3 camForward = cam.forward;
        camForward.y = 0;
        camForward.Normalize();

        // use our camera's right vector, which is always horizontal
        Vector3 camRight = cam.right;

        delta = (moveInput.x * camRight + moveInput.y * camForward) * speed;

        if (isGrounded || moveInput.x != 0 || moveInput.y != 0)
        {
            velocity.x = delta.x;
            velocity.z = delta.z;
        }

        // rotate the render component according to the camera in turn affecting beta's rotation.
        transform.forward = camForward;

        // check for jumping
        if (jumpInput && isGrounded)
            velocity.y = Mathf.Sqrt(2 * jumpHeight * gravity);

        // check if we've hit ground from falling. If so, remove our velocity
        if (isGrounded && velocity.y < 0)
            velocity.y = 0;

        // apply gravity I was wondering why this wasn't after zeroing 
        velocity += Physics.gravity * Time.fixedDeltaTime;


        // and apply this to our positional update this frame
        delta += velocity * Time.fixedDeltaTime;
        // I don't see how this is better the code already had air control due to not restricting horizontal movement all that has been 
        //done is to shift the values in delta into velocity every fixed frame.

        if (!isGrounded)
            hitDirection = Vector3.zero;

        // slide objects off surfaces they're hanging on to
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            Vector3 horizontalHitDirection = hitDirection;
            horizontalHitDirection.y = 0;
            float displacement = horizontalHitDirection.magnitude;
            if (displacement > 0)
                velocity -= 0.2f * horizontalHitDirection / displacement;
        }

        cc.Move(velocity * Time.deltaTime);
        isGrounded = cc.isGrounded;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitDirection = hit.point - transform.position;
        if (hit.rigidbody)
        {
            hit.rigidbody.AddForceAtPosition(velocity * mass, hit.point);
        }
    }
}
