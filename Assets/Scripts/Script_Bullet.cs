using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    void Update()
    {
        transform.position += transform.forward * speed;
    }
}
