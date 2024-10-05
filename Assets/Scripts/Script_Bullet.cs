using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Script_Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private float lifespan = 5;
    public float damages = 10;
    [SerializeField] private Vector3 oldPos;
    [SerializeField] private VisualEffect vfx;
    public string from;

    private void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(oldPos, transform.position - oldPos, out hit,Time.deltaTime))
        {
            Debug.Log("Entered Collider : " + hit.transform.tag);
            if (hit.transform.tag == "Map")
            {
                StartCoroutine("Die");
            }
        }

        lifespan -= Time.deltaTime;
        if (lifespan <= 0) { StartCoroutine("Die"); }

        Debug.DrawRay(oldPos, transform.position - oldPos, Color.yellow);
        oldPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != from && other.transform.tag != "Bullet")
        {
            if(other.transform.tag == "Ennemy")
            {
                other.gameObject.GetComponent<Script_Ennemy>().TakeDamage(damages);
            }
            StartCoroutine("Die");
        }
    }

    private IEnumerator Die()
    {
        speed = 0;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        vfx.Play();
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
