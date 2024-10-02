using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Script_Gun : MonoBehaviour
{
    [SerializeField] private bool isOnGround = true;
    [SerializeField] private int ammunitions = 25;
    [SerializeField] private int errorAngle = 25;
    [SerializeField] private float errorSpeed = 0.5f;
    [SerializeField] private float shootingSpeed = 0.2f;

    [SerializeField] private float currentTime = 0.0f;
    [SerializeField] private float currentErrorAngle = 0.0f;
    [SerializeField] GameObject bulletPREFAB;

    [SerializeField] private VisualEffect vfx;

    private void Start()
    {
        vfx = transform.GetComponentInChildren<VisualEffect>();
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if(currentErrorAngle > 0)
        {
            currentErrorAngle -= Time.deltaTime * 5;
        }
        Debug.DrawRay(transform.position, Quaternion.Euler(0, currentErrorAngle, 0) * transform.up , Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -currentErrorAngle, 0) * transform.up, Color.red);
    }

    public bool Shoot()
    {
        if (currentTime <= 0 && ammunitions != 0)
        {
            int errorMargin = Random.Range((int)-currentErrorAngle, (int)currentErrorAngle + 1);
            GameObject bullet = Instantiate(bulletPREFAB, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + errorMargin, transform.eulerAngles.z));
            currentTime = shootingSpeed;
            currentErrorAngle += errorSpeed;
            currentErrorAngle = Mathf.Clamp(currentErrorAngle, 0, errorAngle);

            vfx.Play();

            return true;
        }
        return false;
    }
}
