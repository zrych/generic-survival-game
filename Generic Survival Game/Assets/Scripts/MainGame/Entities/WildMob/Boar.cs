using System.Collections;
using UnityEngine;

public class Boar : WildMob
{
    public enum State { Idle, Patrol, Aggro }
    public State state { get; private set; } = State.Idle;

    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 3f;
    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 3f;
    [SerializeField] private float stoppingDistance = 0.1f;

    [Header("Aggro")]
    [SerializeField] private float aggroDuration = 4f;
    [SerializeField] private float chaseSpeed = 3f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 0.7f;
    [SerializeField] private float attackDamage = 0.5f;
    [SerializeField] private float attackCooldown = 1.0f;

    [Header("Idle Sound")]
    [SerializeField] private string boarSoundID = "Boar";
    [SerializeField] private float minIdleSoundDelay = 4f;
    [SerializeField] private float maxIdleSoundDelay = 9f;
    [SerializeField] private float maxHearingDistance = 6f;

    private float idleTimer;
    private float aggroTimer;
    private bool canAttack = true;

    private float idleSoundTimer;

    private Transform player;
    private SpriteRenderer sr;

    private Vector2 spawnPoint;
    private Vector2 patrolTarget;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float obstacleCheckDistance = 0.4f;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindWithTag("Player")?.transform;
        sr = GetComponent<SpriteRenderer>();

        spawnPoint = transform.position;
        ChooseIdle();
        ResetIdleSoundTimer();
    }

    private void ChooseIdle()
    {
        idleTimer = UnityEngine.Random.Range(minIdleTime, maxIdleTime);
    }

    private void ResetIdleSoundTimer()
    {
        idleSoundTimer = UnityEngine.Random.Range(minIdleSoundDelay, maxIdleSoundDelay);
    }

    void Update()
    {
        UpdateAnimator();
        HandleSpriteFlip();
        HandleIdleSound();

        switch (state)
        {
            case State.Idle:
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0f)
                    StartPatrol();
                break;

            case State.Patrol:
                if (Vector2.Distance(transform.position, patrolTarget) <= stoppingDistance)
                {
                    state = State.Idle;
                    ChooseIdle();
                }
                break;

            case State.Aggro:
                aggroTimer -= Time.deltaTime;

                if (aggroTimer <= 0f)
                {
                    state = State.Idle;
                    ChooseIdle();
                }
                else
                {
                    TryAttack();
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (state == State.Patrol)
        {
            MoveToward(patrolTarget, moveSpeed);
        }
        else if (state == State.Aggro && player != null)
        {
            MoveToward(player.position, chaseSpeed);
        }
        else if (state == State.Idle)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.5f);
        }
    }

    private void StartPatrol()
    {
        patrolTarget = spawnPoint + UnityEngine.Random.insideUnitCircle * patrolRadius;
        state = State.Patrol;
    }

    public void OnHit(Vector2 attackerPos)
    {
        state = State.Aggro;
        aggroTimer = aggroDuration;
    }

    private void TryAttack()
    {
        if (!canAttack || player == null)
            return;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;

        PlayerStats hp = player.GetComponent<PlayerStats>();
        if (hp != null)
            hp.TakeDamage(attackDamage);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void MoveToward(Vector2 target, float speed)
    {
        Vector2 dir = target - rb.position;

        if (dir.sqrMagnitude < 0.01f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        dir.Normalize();

        if (IsObstacleAhead(dir))
        {
            patrolTarget = spawnPoint + UnityEngine.Random.insideUnitCircle * patrolRadius;
            return;
        }

        rb.linearVelocity = dir * speed;
    }

    private bool IsObstacleAhead(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            obstacleCheckDistance,
            obstacleMask
        );
        return hit.collider != null;
    }

    private void HandleIdleSound()
    {
        if (state == State.Aggro || SoundManager.Instance == null || player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > maxHearingDistance)
            return;

        idleSoundTimer -= Time.deltaTime;
        if (idleSoundTimer <= 0f)
        {
            SoundManager.Instance.PlaySound3D(boarSoundID, transform.position);
            ResetIdleSoundTimer();
        }
    }

    private void HandleSpriteFlip()
    {
        if (rb.linearVelocity.x > 0.05f)
            sr.flipX = true;
        else if (rb.linearVelocity.x < -0.05f)
            sr.flipX = false;
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", rb.linearVelocity.magnitude);
        animator.SetBool("IsAggro", state == State.Aggro);
    }
}
