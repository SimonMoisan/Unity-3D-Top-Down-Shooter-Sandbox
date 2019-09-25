using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyMovement : MonoBehaviour
{
    [Header("Associated objects :")]
    public NavMeshAgent agent;
    public Transform target;
    public Animator animator;

    [Header("Ennemy caracteristics :")]
    [SerializeField] public float health;
    [SerializeField] public float attackRate;
    private float attackCountdown = 0f;

    [Header("Ennemy stats :")]
    public bool isAttacking = false;
    public bool isChasing = false;

    public 

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>().transform;
        animator = gameObject.GetComponent<Animator>();

        animator.SetBool("Run Forward", true);
        isChasing = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, target.position.z);

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if(distanceToPlayer <= 4f)
        {
            isAttacking = true;
            agent.isStopped = true;

            if(attackCountdown <= 0)
            {
                isAttacking = true;
                AttackPlayer();
                attackCountdown = 1 / attackRate;
            }
            attackCountdown -= Time.deltaTime;
        }
        else
        {
            agent.isStopped = false;
            ChasePlayer(targetPos);
        }
    }

    void ChasePlayer(Vector3 targetPos)
    {
        animator.SetBool("Run Forward", true);
        agent.SetDestination(targetPos);
    }

    void AttackPlayer()
    {
        if(isAttacking)
        {
            animator.SetBool("Run Forward", false);
            animator.SetTrigger("Stab Attack");
            isAttacking = false;
        }
    }
}
