using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private float speed = 1.0f;

    [Header("Debug")]
    [SerializeField] private Vector2 inputMovement;
    [SerializeField] private bool inputShooting;
    [SerializeField] private bool inputInteract;
    [SerializeField] private player_inputs playerInput;


    private void Start()
    {
        playerInput = GetComponent<player_inputs>();
    }
    void Update()
    {
        FollowCursor();
        GetInputs();
    }

    private void FollowCursor()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit) )
        {
            playerVisual.transform.LookAt(hit.point);
            playerVisual.transform.eulerAngles = new Vector3(90, playerVisual.transform.eulerAngles.y, 0);
        }
    }

    void GetInputs()
    {
        inputMovement = playerInput.movement;
        Vector3 nextMovement = new Vector3(inputMovement.x, 0, inputMovement.y) * speed * Time.deltaTime;
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, nextMovement, out hit, Mathf.Infinity, 0))
        {
            transform.position += nextMovement;
        }
        else { transform.position += nextMovement * hit.distance; }
        Debug.DrawRay(transform.position, nextMovement * 10, Color.yellow);

        inputInteract = playerInput.isInteracting;

        inputShooting = playerInput.isShooting;
    }
}
