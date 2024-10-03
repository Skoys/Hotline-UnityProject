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
    [SerializeField] EnnemyBehaviour behaviour = EnnemyBehaviour.FindPath;
    [SerializeField] Vector3 gotoNext;

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

    // Update is called once per frame
    void Update()
    {
        switch (behaviour)
        {
            case EnnemyBehaviour.Attack:
                return;

            case EnnemyBehaviour.GetNear:
                return;

            case EnnemyBehaviour.FindPath:
                return;
        }
    }

    private void FindPath()
    {

    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(gotoNext, 0.5f);
    }
}
