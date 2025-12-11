using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;

    private float speedX, speedY;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    public bool IsSprinting { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();      // get the Animator component
        sr = GetComponent<SpriteRenderer>();  // get SpriteRenderer for flipping
    }

    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");

        IsSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = IsSprinting ? sprintSpeed : walkSpeed;

        // apply movement
        Vector2 moveInput = new Vector2(speedX, speedY).normalized;
        rb.linearVelocity = moveInput * currentSpeed;

        // send movement values to animator
        anim.SetFloat("MoveX", speedX);
        anim.SetFloat("MoveY", speedY);
        anim.SetFloat("Speed", rb.linearVelocity.sqrMagnitude);
        anim.SetBool("IsSprinting", IsSprinting);
    }
}
