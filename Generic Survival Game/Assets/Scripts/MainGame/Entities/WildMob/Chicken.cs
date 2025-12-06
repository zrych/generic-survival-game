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
    [SerializeField] private float timeBetweenPatrols = 0.5f;

    [Header("Panic")]
    [SerializeField] private float panicDuration = 3.0f;      // how long to flee
    [SerializeField] private float panicRepathInterval = 0.4f; // update flee direction occasionally

    public State state { get; private set; } = State.Idle;
    private Vector2 spawnPoint;
    private Vector2 patrolTarget;
    private float idleTimer;
    private float panicTimer;
    private Coroutine panicCoroutine;
    private Transform playerTransform; // cached player (optional)
    private Vector2 lastVelocity;
    private float speedX, speedY;
    private SpriteRenderer sr;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float obstacleCheckDistance = 0.4f;

    public enum State { Idle, Patrol, Panic }

    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;
        ChooseNextIdle(); // start with idle/patrol flow
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        // default to Patrol after initial idle
        state = State.Idle;
        sr = GetComponent<SpriteRenderer>();
    }

    private void ChooseNextIdle()
    {
        idleTimer = Random.Range(minIdleTime, maxIdleTime);
        // after idle completes, we'll go to patrol
    }

    void Update()
    {
        // Update animator params based on velocity
        float speed = rb.linearVelocity.magnitude;
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
            // optional directional params if you use them
            animator.SetFloat("MoveX", rb.linearVelocity.x);
            animator.SetFloat("MoveY", rb.linearVelocity.y);
            animator.SetBool("IsPanicking", state == State.Panic);
        }
        else Debug.Log("No Animator component in Chicken.cs!");

        if (rb.linearVelocity.x > 0)
        sr.flipX = true;
        else if (rb.linearVelocity.x < 0)
        sr.flipX = false;

        // Only run high-level state logic in Update (physics movement in FixedUpdate)
        if (state == State.Idle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f)
            {
                StartPatrol();
            }
        }
        else if (state == State.Patrol)
        {
            // if close to target, wait then pick another
            float dist = Vector2.Distance(transform.position, patrolTarget);
            if (dist <= stoppingDistance)
            {
                // reached target -> idle for a bit then continue patrolling
                state = State.Idle;
                ChooseNextIdle();
            }
        }
        else if (state == State.Panic)
        {
            // panicTimer is handled by coroutine, but we can check fallback
            if (panicTimer <= 0f && panicCoroutine == null)
            {
                EndPanic();
            }
        }
    }

    void FixedUpdate()
    {
        // Movement is physics-based here
        if (state == State.Patrol)
        {
            MoveToward(patrolTarget, moveSpeed);
        }
        else if (state == State.Idle)
        {
            // gently stop
            rb.MovePosition(rb.position + Vector2.zero);
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.6f);
        }
        else if (state == State.Panic)
        {
            // panic movement is handled by coroutine which sets rb.velocity directly via MoveTowards or MovePosition
            // but ensure animator sees velocity
        }

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
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            obstacleCheckDistance,
            obstacleMask
        );

        return hit.collider != null;
    }

    private void MoveToward(Vector2 worldTarget, float speed)
    {
        Vector2 dir = ((Vector2)worldTarget - rb.position);
        if (dir.magnitude <= 0.001f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        dir.Normalize();
        if (IsObstacleAhead(dir))
        {
            patrolTarget = RandomPointInCircle(spawnPoint, patrolRadius);
            return;
        }
        rb.linearVelocity = dir * speed;
    }

    private Vector2 RandomPointInCircle(Vector2 center, float radius)
    {
        Vector2 rand = Random.insideUnitCircle * radius;
        return center + rand;
    }

    public void OnHit(Vector2 attackerPosition)
    {
        Debug.Log(attackerPosition);
        // stop any idle/patrol coroutines
        if (panicCoroutine != null)
        {
            StopCoroutine(panicCoroutine);
            panicCoroutine = null;
        }

        panicCoroutine = StartCoroutine(PanicRoutine(attackerPosition));
    }

    private IEnumerator PanicRoutine(Vector2 attackerPos)
    {
        state = State.Panic;
        panicTimer = panicDuration;

        float elapsed = 0f;
        // initial flee direction: away from attacker
        Vector2 fleeDir = ((Vector2)transform.position - attackerPos).normalized;
        if (fleeDir.sqrMagnitude < 0.01f) // attacker on top: choose random direction
            fleeDir = Random.insideUnitCircle.normalized;

        // Recompute flee direction occasionally so chicken doesn't get stuck
        while (elapsed < panicDuration)
        {
            // set flee dir away from attacker (re-evaluate attacker pos if player exists)
            if (playerTransform != null)
                fleeDir = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;


            if (IsObstacleAhead(fleeDir))
            {
                // 90-degree turn left/right
                fleeDir = Vector2.Perpendicular(fleeDir).normalized;

                // randomize between left/right
                if (Random.value > 0.5f)
                    fleeDir = -fleeDir;
            }

            rb.linearVelocity = fleeDir * panicSpeed;

            // small jitter to avoid straight-line AI
            rb.linearVelocity += Random.insideUnitCircle * 0.2f;

            elapsed += panicRepathInterval;
            panicTimer -= panicRepathInterval;
            yield return new WaitForSeconds(panicRepathInterval);
        }

        // stop panicking and recover
        EndPanic();
    }
}
