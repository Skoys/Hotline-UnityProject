using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Gun : MonoBehaviour
{
    public bool isOnGround = true;
    public int ammunitions = 25;
    public int errorAngle = 25;
    public float shootingSpeed = 0.2f;

    private float currentTime = 0.0f;
    [SerializeField] GameObject bulletPREFAB;

    public void Shoot()
    {
        if (currentTime <= 0 && ammunitions != 0)
        {
            GameObject bullet = Instantiate(bulletPREFAB, transform.position, transform.rotation);
            currentTime = shootingSpeed;
        }
        currentTime -= Time.deltaTime;
    }
}
