using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    private const string MoveHash = "Move";

    enum State { Idle, Move, Die }

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
                    _currentState = State.Idle;
                    break;
                case State.Move:
                    animator.SetBool(MoveHash, true);
                    _currentState = State.Move;
                    break;
                case State.Die:
                    animator.SetTrigger("Die");
                    _currentState = State.Die;
                    break;
            }
        }
    }

    private NavMeshAgent agent;
    public GameObject target;

    private Animator animator;

    public int Health { get; private set; }
    private bool isDead;

    public ParticleSystem HitEffect;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        CurrentState = State.Move;
        isDead = false;
    }

    void Update()
    {
        switch (_currentState)
        {
            case State.Idle:
                IdleUpdate();
                break;

            case State.Move:
                MoveUpdate();
                break;

            case State.Die:
                if (isDead) break;
                Die(); break;
        }
    }

    public void IdleUpdate()
    {

    }

    public void MoveUpdate()
    {      
        agent.SetDestination(target.transform.position);
    }

    public void Die()
    {
        isDead = true;
        // 점수 올리기?
    }

    public void OnDamage(int damage, Vector3 position, Vector3 normal)
    {
        Health -= damage;

        HitEffect.transform.position = position;
        HitEffect.transform.forward = normal;
        HitEffect.Play();
    }
}
