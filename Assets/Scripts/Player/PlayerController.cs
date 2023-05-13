using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputMap InputMap;
    public float MoveSpeed;
    public int PlayerNumber;

    private Vector3 moveDirection;
    private NavMeshAgent agent;
    private Transform handPivot;
    private Transform interactable;
    private bool hasTablette;
    private float holdTime;

    private bool isDashing;
    private Vector3 dahsDirection;
    public float dashSpeed;
    public float dashTime = 0.5f;
    private float dashTimer;

    public float HoldTime
    {
        get => holdTime;
        set
        {
            holdTime = value;
            GameManager.Instance.UpdatePlayerHoldTime(PlayerNumber, holdTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitilizeInputs();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        handPivot = transform.Find("HandsPivot");
        interactable = GameObject.Find("[Interactable]").transform;
        GameManager.Instance.RegisterPlayer(PlayerNumber);
    }

    private void InitilizeInputs()
    {
        InputMap.MoveUp.Enable();
        InputMap.MoveDown.Enable();
        InputMap.MoveLeft.Enable();
        InputMap.MoveRight.Enable();
        InputMap.Interact.Enable();

        InputMap.MoveUp.performed += ctx => OnMoveUp(ctx);
        InputMap.MoveDown.performed += ctx => OnMoveDown(ctx);
        InputMap.MoveLeft.performed += ctx => OnMoveLeft(ctx);
        InputMap.MoveRight.performed += ctx => OnMoveRight(ctx);
        InputMap.Interact.performed += ctx => Interact(ctx);

        InputMap.MoveUp.canceled += ctx => OnMoveUp(ctx);
        InputMap.MoveDown.canceled += ctx => OnMoveDown(ctx);
        InputMap.MoveLeft.canceled += ctx => OnMoveLeft(ctx);
        InputMap.MoveRight.canceled += ctx => OnMoveRight(ctx);
        InputMap.Interact.canceled += ctx => Interact(ctx);
    }
    public void FixedUpdate()
    {
        if (isDashing)
        {
            agent.Move(dahsDirection.normalized * dashSpeed);
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= dashTime)
            {
                isDashing = false;
                dashTimer = 0;
            }
        }
        else agent.Move(moveDirection.normalized * MoveSpeed);
        if (moveDirection != Vector3.zero) transform.forward = moveDirection.normalized;
        if (hasTablette) HoldTime += Time.fixedDeltaTime;
    }
    public void OnMoveUp(InputAction.CallbackContext context)
    {
        if (context.performed) moveDirection.z = 1;
        if (context.canceled && moveDirection.z == 1) moveDirection.z = 0;
    }
    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if (context.performed) moveDirection.z = -1;
        if (context.canceled && moveDirection.z == -1) moveDirection.z = 0;
    }
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed) moveDirection.x = -1;
        if (context.canceled && moveDirection.x == -1) moveDirection.x = 0;
    }
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed) moveDirection.x = 1;
        if (context.canceled && moveDirection.x == 1) moveDirection.x = 0;
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.canceled) return;
        if (hasTablette)
        {
            dahsDirection = moveDirection;
            isDashing = true;
            dashTimer = 0;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision " + collision.gameObject.tag);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<Interactable>().OnCollide(this);
        }
    }

    public void GrabTablette(Transform tablette)
    {
        tablette.gameObject.transform.parent = handPivot;
        tablette.gameObject.transform.up = transform.forward;
        tablette.gameObject.transform.forward = Vector3.up;
        tablette.transform.localPosition = Vector3.zero;
        hasTablette = true;
    }
    public void DropTablette()
    {
        hasTablette = false;
    }

}
