using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, IDamageable
{
    private const string MoveHash = "Move";

    enum State { Idle, Move, Attack, Die }

    private State _currentState;
    private State CurrentState
    {
        get { return _currentState; }
        set
        {
            switch (value)
            {
                case State.Idle:
                    animator.SetBool(MoveHash, false);
                    agent.isStopped = true;
                    _currentState = State.Idle;
                    break;
                case State.Move:
                    animator.SetBool(MoveHash, true);
                    agent.isStopped = false;
                    _currentState = State.Move;
                    break;
                case State.Attack:
                    animator.SetBool(MoveHash, true);
                    agent.isStopped = true;
                    _currentState = State.Attack;
                    break;
                case State.Die:
                    animator.SetTrigger("Die");
                    agent.isStopped = true;
                    _currentState = State.Die;
                    break;
            }
        }
    }

    public NavMeshAgent agent;
    public GameObject target;
    public HitBox hitBox;

    private Animator animator;

    public int Health { get; set; }
    public int AttackPower { get; set; }

    private bool isDead;

    public ParticleSystem HitEffect;

    private Rigidbody enemyRigidbody;
    private CapsuleCollider enemyCollider;

    private Vector3 targetPos;

    private float lastAttackTime;
    private float attackInterval = 2.5f;
    private float attackDistance = 1.5f;

    public GameManager gameManager;

    public int addScore;

    public bool isPaused;

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<CapsuleCollider>();

        CurrentState = State.Move;
        isDead = false;
    }

    void Update()
    {
        if (gameManager.isPaused && !isPaused)
        {
            OnPause();
            return;
        }

        else if (!gameManager.isPaused && isPaused)
        {
            OffPause();
        }

        if (isPaused) return;

        switch (_currentState)
        {
            case State.Idle:
                IdleUpdate();
                break;

            case State.Move:
                MoveUpdate();
                break;

            case State.Attack:
                AttackUpdate();
                break;

            case State.Die:
                if (!isDead) Die();
                else DieUpdate();
                break;
        }
    }

    public void IdleUpdate()
    {
        return;
    }

    public void MoveUpdate()
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
        {
            CurrentState = State.Attack;
            return;
        }

        agent.SetDestination(target.transform.position);
    }

    public void AttackUpdate()
    {
        if (Vector3.Distance(target.transform.position, transform.position) > attackDistance)
        {
            CurrentState = State.Move;
            return;
        }

        if (Time.time > lastAttackTime + attackInterval)
        {
            var attackTarget = target.gameObject.GetComponent<IDamageable>();

            if (attackTarget != null)
            {
                attackTarget.OnDamage(AttackPower, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            }

            lastAttackTime = Time.time;
        }
    }

    public void DieUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);
    }

    public void Die()
    {
        isDead = true;
        targetPos = new(transform.position.x, -3, transform.position.z);

        gameManager.AddScore(addScore);
    }

    public void OnDamage(int damage, Vector3 position, Vector3 normal)
    {
        if (isDead) return;

        Health -= damage;

        HitEffect.transform.position = position;
        HitEffect.transform.forward = normal;
        HitEffect.Play();

        if (Health <= 0)
        {
            CurrentState = State.Die;
        }
    }

    public void StartSinking()
    {
        Destroy(gameObject, 3);
    }

    public void OnPause()
    {
        isPaused = true;
        animator.speed = 0;
        agent.isStopped = true;
        CurrentState = State.Idle;
    }

    public void OffPause()
    {
        isPaused = false;
        animator.speed = 1;
        agent.isStopped = false;
        CurrentState = State.Move;
    }
}