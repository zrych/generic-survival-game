using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4;

    private float speedX, speedY;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

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

        // apply movement
        rb.linearVelocity = new Vector2(speedX * moveSpeed, speedY * moveSpeed);

        // send movement values to animator
        anim.SetFloat("MoveX", speedX);
        anim.SetFloat("MoveY", speedY);
        anim.SetFloat("Speed", rb.linearVelocity.sqrMagnitude);

        // flip sprite for left movement (if you only have right-facing art)
        if (speedX < 0)
            sr.flipX = true;
        else if (speedX > 0)
            sr.flipX = false;
    }
}
