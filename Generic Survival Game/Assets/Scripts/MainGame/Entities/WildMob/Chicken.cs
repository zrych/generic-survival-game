using System.Collections;
using UnityEngine;

public class Chicken : WildMob
{
    [Header("Stats")]
    [SerializeField] private float panicSpeed = 4f;
    [SerializeField] private float stoppingDistance = 0.1f;

    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 3f;
    [SerializeField] private float minIdleTime = 1.0f;
    [SerializeField] private float maxIdleTime = 3.0f;

    [Header("Panic")]
    [SerializeField] private float panicDuration = 3.0f;
    [SerializeField] private float panicRepathInterval = 0.4f;

    [Header("Audio")]
    [SerializeField] private float minIdleSoundDelay = 3f;
    [SerializeField] private float maxIdleSoundDelay = 8f;
    [SerializeField] private float maxHearingDistance = 5f;

    public State state { get; private set; } = State.Idle;

    private Vector2 spawnPoint;
    private Vector2 patrolTarget;
    private float idleTimer;
    private float panicTimer;
    private Coroutine panicCoroutine;
    private Transform playerTransform;
    private Vector2 lastVelocity;
    private SpriteRenderer sr;

    private float idleSoundTimer;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float obstacleCheckDistance = 0.4f;

    public enum State { Idle, Patrol, Panic }

    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;
        ChooseNextIdle();
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        state = State.Idle;
        sr = GetComponent<SpriteRenderer>();

        idleSoundTimer = UnityEngine.Random.Range(minIdleSoundDelay, maxIdleSoundDelay);
    }

    private void ChooseNextIdle()
    {
        idleTimer = UnityEngine.Random.Range(minIdleTime, maxIdleTime);
    }

    void Update()
    {
        // Update animator
        float speed = rb.linearVelocity.magnitude;
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
            animator.SetFloat("MoveX", rb.linearVelocity.x);
            animator.SetFloat("MoveY", rb.linearVelocity.y);
            animator.SetBool("IsPanicking", state == State.Panic);
        }

        if (rb.linearVelocity.x > 0) sr.flipX = true;
        else if (rb.linearVelocity.x < 0) sr.flipX = false;

        // State logic
        if (state == State.Idle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f) StartPatrol();
        }
        else if (state == State.Patrol)
        {
            float dist = Vector2.Distance(transform.position, patrolTarget);
            if (dist <= stoppingDistance)
            {
                state = State.Idle;
                ChooseNextIdle();
            }
        }
        else if (state == State.Panic)
        {
            if (panicTimer <= 0f && panicCoroutine == null) EndPanic();
        }

        PlayIdleSound();
    }

    void FixedUpdate()
    {
        if (state == State.Patrol) MoveToward(patrolTarget, moveSpeed);
        else if (state == State.Idle) rb.MovePosition(rb.position + Vector2.zero);

        lastVelocity = rb.linearVelocity;
    }

    private void StartPatrol()
    {
        patrolTarget = RandomPointInCircle(spawnPoint, patrolRadius);
        state = State.Patrol;
    }

    private void EndPanic()
    {
        panicTimer = 0f;
        panicCoroutine = null;
        state = State.Idle;
        ChooseNextIdle();
    }

    private bool IsObstacleAhead(Vector2 direction)
    {
        return Physics2D.Raycast(transform.position, direction, obstacleCheckDistance, obstacleMask).collider != null;
    }

    private void MoveToward(Vector2 worldTarget, float speed)
    {
        Vector2 dir = worldTarget - rb.position;
        if (dir.magnitude <= 0.001f) { rb.linearVelocity = Vector2.zero; return; }
        dir.Normalize();
        if (IsObstacleAhead(dir)) { patrolTarget = RandomPointInCircle(spawnPoint, patrolRadius); return; }
        rb.linearVelocity = dir * speed;
    }

    private Vector2 RandomPointInCircle(Vector2 center, float radius)
    {
        return center + UnityEngine.Random.insideUnitCircle * radius;
    }

    public void OnHit(Vector2 attackerPosition)
    {
        if (panicCoroutine != null) StopCoroutine(panicCoroutine);
        panicCoroutine = StartCoroutine(PanicRoutine(attackerPosition));
    }

    private IEnumerator PanicRoutine(Vector2 attackerPos)
    {
        state = State.Panic;
        panicTimer = panicDuration;
        float elapsed = 0f;

        Vector2 fleeDir = ((Vector2)transform.position - attackerPos).normalized;
        if (fleeDir.sqrMagnitude < 0.01f) fleeDir = UnityEngine.Random.insideUnitCircle.normalized;

        while (elapsed < panicDuration)
        {
            if (playerTransform != null)
                fleeDir = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;

            if (IsObstacleAhead(fleeDir))
            {
                fleeDir = Vector2.Perpendicular(fleeDir).normalized;
                if (UnityEngine.Random.value > 0.5f) fleeDir = -fleeDir;
            }

            rb.linearVelocity = fleeDir * panicSpeed;
            rb.linearVelocity += UnityEngine.Random.insideUnitCircle * 0.2f;

            elapsed += panicRepathInterval;
            panicTimer -= panicRepathInterval;
            yield return new WaitForSeconds(panicRepathInterval);
        }

        EndPanic();
    }

    private void PlayIdleSound()
    {
        if (SoundManager.Instance == null || playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance > maxHearingDistance) return;

        idleSoundTimer -= Time.deltaTime;
        if (idleSoundTimer <= 0f)
        {
            float volume = 1f - (distance / maxHearingDistance); // fade out with distance
            SoundManager.Instance.PlaySound2D("Chicken"); // your SoundManager will randomly pick a clip

            idleSoundTimer = UnityEngine.Random.Range(minIdleSoundDelay, maxIdleSoundDelay);
        }
    }
}
