using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float maxSpeed = 10.0f;

    [SerializeField] private float interactDist = 2f;

    [Header("Gun")]
    [SerializeField] private GameObject gunEmplacement;
    [SerializeField] private bool hasGun = false;
    [SerializeField] private LayerMask gunLayer;
    

    [Header("Debug")]
    [SerializeField] private Vector2 inputMovement;
    [SerializeField] private bool inputShooting;
    [SerializeField] private bool inputInteract;
    [SerializeField] private player_inputs playerInput;
    [SerializeField] private Rigidbody rb;
    private Animator animator;

    private void Start()
    {
        playerInput = GetComponent<player_inputs>();
        animator = playerVisual.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        FollowCursor();
        GetInputs();
        Movements();
        Interact();
        Shoot();
        Animations();
    }

    private void FollowCursor()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit) )
        {
            playerVisual.transform.LookAt(hit.point);
            playerVisual.transform.eulerAngles = new Vector3(90, playerVisual.transform.eulerAngles.y - 1, 0);
            Debug.DrawRay(hit.point, new Vector3(0, 1, 0), Color.red);
        }
    }

    void GetInputs()
    {
        inputMovement = playerInput.movement;
        inputInteract = playerInput.isInteracting;
        inputShooting = playerInput.isShooting;
    }

    void Movements()
    {
        Vector3 nextMovement = new Vector3(inputMovement.x, 0, inputMovement.y) * (speed * 0.1f);
        RaycastHit hit;
        rb.velocity = nextMovement * speed;
        Debug.DrawRay(transform.position, rb.velocity, Color.yellow);

        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));
    }

    void Interact()
    {
        if(!inputInteract) { return; }
        Collider[] list = Physics.OverlapSphere(transform.position, interactDist, gunLayer.value);
        if(list.Length == 0 ) { return; }
        GameObject closest = list[0].gameObject;
        foreach (Collider collider in list)
        {
            if(Vector3.Distance(collider.transform.position, transform.position) < Vector3.Distance(closest.transform.position, transform.position))
            {
                closest = collider.gameObject;
            }
        }
        closest.transform.SetParent(gunEmplacement.transform);
        closest.transform.localPosition = Vector3.zero;
        closest.transform.localEulerAngles = new Vector3(90, 0, 270);
        hasGun = true;
    }

    void Shoot()
    {
        if (!inputShooting) { return; }
        if(gunEmplacement.transform.childCount == 0) { return; }
        bool hasShot = gunEmplacement.GetComponentInChildren<Script_Gun>().Shoot();
        if (!hasShot) { return; }
        IEnumerator coroutine = ShakeCamera(1, 0.15f);
        StartCoroutine(coroutine);
    }

    void Animations()
    {
        animator.SetFloat("animWalkSpeed", Mathf.Abs(inputMovement.x + inputMovement.y));
        animator.SetBool("hasGun", hasGun);
    }

    private IEnumerator ShakeCamera(float intensity, float timer)
    {
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(timer);
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactDist);
    }
}
