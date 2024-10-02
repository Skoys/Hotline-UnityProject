using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private Vector3 oldPos;


    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(oldPos, transform.position - oldPos, out hit,Time.deltaTime))
        {
            Debug.Log("Entered Collider : " + hit.transform.tag);
            if (hit.transform.tag == "Map")
            {
                Destroy(gameObject);
            }
        }
        Debug.DrawRay(oldPos, transform.position - oldPos, Color.yellow);
        oldPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Map")
        {
            Destroy(gameObject);
        }
    }
}
