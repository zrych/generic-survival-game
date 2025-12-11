using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Zombie : Monster
{
    public enum State { Idle, Patrol, Aggro, RunAway }
    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 3f;
    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 3f;
    [SerializeField] private float stoppingDistance = 0.1f;

    private Transform player;
    private SpriteRenderer sr;
    public State state { get; private set; } = State.Idle;

    private Vector2 spawnPoint;
    private Vector2 patrolTarget;

    private float idleTimer;
    private bool canAttack = true;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float obstacleCheckDistance = 0.4f;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindWithTag("Player")?.transform;
        sr = GetComponent<SpriteRenderer>();

        spawnPoint = gameObject.transform.position;
        ChooseIdle();
    }

    private void ChooseIdle()
    {
        idleTimer = Random.Range(minIdleTime, maxIdleTime);
    }

    private Vector2 RandomPatrolPoint()
    {
        return spawnPoint + Random.insideUnitCircle * patrolRadius;
    }

    void Update()
    {
        UpdateAnimator();
        HandleSpriteFlip();
        if (IsPlayerDetected() && !PlayerIsInCampfireRadius()) state = State.Aggro;
        switch (state)
        {
            case State.Idle:
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0f)
                {
                    StartPatrol();
                }
                break;

            case State.Patrol:
                float dist = Vector2.Distance(transform.position, patrolTarget);
                if (dist <= stoppingDistance)
                {
                    state = State.Idle;
                    ChooseIdle();
                }
                break;

            case State.Aggro:
                if (ZombieIsInCampfireRadius()) {
                    state = State.RunAway;
                }
                TryAttack();
                break;
        }
    }

    private bool IsPlayerDetected()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= detectionRange;
    }

    void FixedUpdate()
    {
        if (state == State.Patrol)
        {
            MoveToward(patrolTarget, moveSpeed);
        }
        else if (state == State.Aggro)
        {
            if (player != null)
                MoveToward(player.position, moveSpeed);
        }
        else if (state == State.Idle)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.5f);
        } else if (state == State.RunAway)
        {
            StartCoroutine(MoveAway(player.transform.position, moveSpeed));
        }
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
        if (IsObstacleAhead(dir) || ZombieIsInCampfireRadius())
        {
            patrolTarget = RandomPatrolPoint();
            return;
        }
        rb.linearVelocity = dir * speed;
    }

    private IEnumerator MoveAway(Vector2 hazard, float speed)
    {
        Vector2 dir = rb.position - hazard;
        if (dir.sqrMagnitude < 0.01f)
        {
            rb.linearVelocity = Vector2.zero;
            yield return null;
        }
        dir.Normalize();
        if (IsObstacleAhead(dir))
        {
            patrolTarget = RandomPatrolPoint();
            yield return null;
        }
        rb.linearVelocity = dir * speed;
        yield return new WaitForSeconds(3f);
        state = State.Idle;
        ChooseIdle();
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

    private bool ZombieIsInCampfireRadius()
    {
        Campfire campfire = Campfire.Instance;
        float distance = Vector2.Distance(transform.position, campfire.transform.position);
        return distance <= campfire.fireRadius;
    }

    private bool PlayerIsInCampfireRadius()
    {
        Campfire campfire = Campfire.Instance;
        float distance = Vector2.Distance(player.position, campfire.transform.position);
        return distance <= campfire.fireRadius;
    }

    private void TryAttack()
    {
        if (!canAttack || player == null)
            return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;

        PlayerStats hp = player.GetComponent<PlayerStats>();
        if (hp != null)
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.4f);
        hp.TakeDamage(attackDamage);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void StartPatrol()
    {
        patrolTarget = RandomPatrolPoint();
        state = State.Patrol;
    }

    private void HandleSpriteFlip()
    {
        if (rb.linearVelocity.x > 0.05f)
            sr.flipX = false;
        else if (rb.linearVelocity.x < -0.05f)
            sr.flipX = true;
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", rb.linearVelocity.magnitude);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
