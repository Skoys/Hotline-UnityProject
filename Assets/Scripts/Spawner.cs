using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float currentTimer = 0;
    [SerializeField] private int timer = 10;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject basicPREFAB;
    [SerializeField] private GameObject heavyPREFAB;

    // Update is called once per frame
    void Update()
    {
        if(currentTimer > timer)
        {
            GameObject ennemy;
            currentTimer = 0;
            ennemy =  Instantiate(basicPREFAB, transform.position + new Vector3(0.25f, 0, 0.25f), Quaternion.identity);
            ennemy.GetComponent<Script_Ennemy>().player = player;
            ennemy = Instantiate(basicPREFAB, transform.position + new Vector3(0.25f,0,-0.25f), Quaternion.identity);
            ennemy.GetComponent<Script_Ennemy>().player = player;
            ennemy = Instantiate(heavyPREFAB, transform.position + new Vector3(-0.25f, 0, 0.25f), Quaternion.identity);
            ennemy.GetComponent<Script_Ennemy>().player = player;
            ennemy = Instantiate(basicPREFAB, transform.position + new Vector3(-0.25f, 0, -0.25f), Quaternion.identity);
            ennemy.GetComponent<Script_Ennemy>().player = player;
        }
        currentTimer += Time.deltaTime;
    }
}
