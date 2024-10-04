using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ennemy : MonoBehaviour
{
    [SerializeField] EnnemyType ennemyType = EnnemyType.Basic;

    [Header("Characteristics")]
    [SerializeField] private float life = 250;
    [SerializeField] private float speed = 5;






    [Header("Debug")]
    [SerializeField] private Script_AI_NavMesh navMesh;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject visual;
    [SerializeField] EnnemyBehaviour behaviour = EnnemyBehaviour.FindPath;
    [SerializeField] Vector3 gotoNext;
    [SerializeField] float time;

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
        Attack
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        visual = GetComponentInChildren<GameObject>();
    }

    void Update()
    {
        switch (behaviour)
        {
            case EnnemyBehaviour.Attack:
                return;

            case EnnemyBehaviour.GetNear:
                GetNear();
                return;

            case EnnemyBehaviour.FindPath:
                FindPath();
                return;
        }
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
        time = Time.time + 1;
        behaviour = EnnemyBehaviour.GetNear;
    }

    private void GetNear()
    {
        if(Vector3.Distance(gotoNext, transform.position) < 1f)
        {
            behaviour = EnnemyBehaviour.FindPath;
        }
        if(time <= Time.time)
        {
            behaviour = EnnemyBehaviour.FindPath;
        }
        Debug.DrawRay(transform.position, rb.velocity, Color.red, 0.1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gotoNext, 1f);
    }
}
