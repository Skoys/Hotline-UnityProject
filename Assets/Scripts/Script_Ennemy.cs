using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ennemy : MonoBehaviour
{
    [SerializeField] EnnemyType ennemyType = EnnemyType.Basic;

    [Header("Characteristics")]
    [SerializeField] private float life = 250;
    [SerializeField] private float speed = 5;
    [SerializeField] private float attackSpeed = 0.3f;
    [SerializeField] private int missAngle = 25;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float lossRange = 15f;




    [Header("Debug")]
    [SerializeField] private Script_AI_NavMesh navMesh;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject player;
    [SerializeField] private Script_Gun gun;
    [SerializeField] private Animator animator;
    [SerializeField] EnnemyBehaviour behaviour = EnnemyBehaviour.FindPath;
    [SerializeField] Vector3 gotoNext;
    [SerializeField] float recalcTime;
    [SerializeField] float AttackTime;

    private enum EnnemyType
    {
        Basic,
        Heavy,
        Medic,
        Shield,
        Cloaker,
        Bulldozer
    }

    private enum EnnemyBehaviour
    {
        FindPath,
        GetNear,
        Attack,
        Dead
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        gun.errorAngle = missAngle;
    }

    void Update()
    {
        switch (behaviour)
        {
            case EnnemyBehaviour.Attack:
                Attack();
                return;

            case EnnemyBehaviour.GetNear:
                GetNear();
                return;

            case EnnemyBehaviour.FindPath:
                FindPath();
                return;
        }
        Animations();
    }

    private void FindPath()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 15))
        {
            if(hit.transform.tag == "Player")
            {
                rb.velocity = (player.transform.position - transform.position).normalized * speed;
                gotoNext = player.transform.position;
            }
            else
            {
                Vector3 target = navMesh.GetTheClosest(gameObject);
                rb.velocity = (target - transform.position).normalized * speed;
                gotoNext = target;
            }
        }
        transform.LookAt(gotoNext);
        transform.eulerAngles = new Vector3(180, transform.eulerAngles.y - 1, 180);
        recalcTime = Time.time + 1;
        behaviour = EnnemyBehaviour.GetNear;
    }

    private void GetNear()
    {
        RaycastHit hit;
        if(Vector3.Distance(gotoNext, transform.position) < 1f)
        {
            behaviour = EnnemyBehaviour.FindPath;
        }
        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            if(Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, attackRange))
            {
                if(hit.collider.tag == "Player")
                {
                    rb.velocity = Vector3.zero;
                    behaviour = EnnemyBehaviour.Attack;
                }
            }
        }
        if (recalcTime <= Time.time)
        {
            behaviour = EnnemyBehaviour.FindPath;
        }
        Debug.DrawRay(transform.position, rb.velocity, Color.red, 0.1f);
    }

    private void Attack()
    {
        RaycastHit hit;
        if (Vector3.Distance(player.transform.position, transform.position) < lossRange && AttackTime <= Time.time)
        {
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, lossRange))
            {
                if (hit.collider.tag != "Player")
                {
                    behaviour = EnnemyBehaviour.FindPath;
                }
                else
                {
                    transform.LookAt(player.transform.position);
                    transform.eulerAngles = new Vector3(180, transform.eulerAngles.y - 1, 180);
                    gun.currentErrorAngle = missAngle;
                    gun.Shoot();
                    AttackTime = Time.time + attackSpeed;
                }
            }
        }
    }

    private void Animations()
    {
        animator.SetFloat("velocity", Mathf.Abs(rb.velocity.x + rb.velocity.z));
    }

    public void TakeDamage(float nbr)
    {
        life -= nbr;
        if(life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        behaviour = EnnemyBehaviour.Dead;
        rb.velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.eulerAngles = new Vector3(180, transform.eulerAngles.y, 180);
        animator.SetTrigger("Die");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gotoNext, 1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lossRange);
    }
}
