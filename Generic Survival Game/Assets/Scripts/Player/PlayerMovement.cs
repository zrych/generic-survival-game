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

    // FOOTSTEP SOUND
    [SerializeField] private string footstepSoundName = "PlayerMovement";
    private AudioSource footstepSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // Create a dedicated AudioSource for footsteps
        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.loop = true;
        footstepSource.playOnAwake = false;

        // Assign clip from SoundManager
        if (SoundManager.Instance != null)
            footstepSource.clip = SoundManager.Instance.GetClipPublic(footstepSoundName);
    }

    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");

        IsSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = IsSprinting ? sprintSpeed : walkSpeed;

        // Apply movement
        Vector2 moveInput = new Vector2(speedX, speedY).normalized;
        rb.linearVelocity = moveInput * currentSpeed;

        // Send movement values to animator
        anim.SetFloat("MoveX", speedX);
        anim.SetFloat("MoveY", speedY);
        anim.SetFloat("Speed", rb.linearVelocity.sqrMagnitude);
        anim.SetBool("IsSprinting", IsSprinting);

        // --- FOOTSTEP SOUND LOGIC ---
        if (moveInput != Vector2.zero)
        {
            if (!footstepSource.isPlaying)
                footstepSource.Play();
        }
        else
        {
            if (footstepSource.isPlaying)
                footstepSource.Stop();
        }
    }
}
