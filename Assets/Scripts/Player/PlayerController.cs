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

    private bool hasTablette;
    public Interactable holdItem;

    private Vector3 moveDirection;
    private NavMeshAgent agent;
    private Transform handPivot;
    private Transform interactable;
    private float holdTime;

    private bool isDashing;
    private Vector3 dahsDirection;
    public float dashSpeed;
    public float dashTime = 0.5f;
    private float dashTimer;

    public bool isKnockedOut;
    private float knockOutTimer;
    private float knockOutDelay;

    public bool hasFreeHands { get { return !hasTablette && holdItem == null; } }

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
        if (isKnockedOut)
        {
            knockOutTimer += Time.fixedDeltaTime;
            if (knockOutTimer >= knockOutDelay)
            {
                agent.enabled = true;
                isKnockedOut = false;
                knockOutTimer = 0;
            }
            return;
        }

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
        if (holdItem != null)
        {
            holdItem.Use(this);
            return;
        }
        TryGrab();
    }

    public void StartDash()
    {
        dahsDirection = moveDirection;
        isDashing = true;
        dashTimer = 0;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision " + collision.gameObject);
        if (collision.gameObject.CompareTag("Interactable"))
        {
            collision.gameObject.GetComponent<Interactable>().OnCollide(this);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger " + other.gameObject);
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<Interactable>().OnCollide(this);
            if (other.TryGetComponent<Rigidbody>(out Rigidbody body))
            {
                Debug.Log(body.velocity.magnitude);
            }
        }
    }

    public void KnockOut(Transform projectil, float duration)
    {
        Debug.Log("knock out " + gameObject.name, gameObject);
        agent.enabled = false;
        isKnockedOut = true;
        knockOutDelay = duration;
        knockOutTimer = 0;
        if (holdItem != null) holdItem.Dropped(this);
    }

    #region tablette
    public void GrabTablette(Transform tablette)
    {
        tablette.gameObject.transform.parent = handPivot;
        tablette.gameObject.transform.up = transform.forward;
        tablette.gameObject.transform.forward = Vector3.up;
        tablette.transform.localPosition = Vector3.zero;
        holdItem = tablette.GetComponent<Interactable>();
        hasTablette = true;
    }
    public void DropTablette()
    {
        hasTablette = false;
        holdItem = null;

    }
    #endregion

    #region Ball

    public void GrabBall(Transform ball)
    {
        ball.gameObject.transform.parent = handPivot;
        ball.transform.localPosition = Vector3.zero;
        ball.GetComponent<Rigidbody>().isKinematic = true;
        ball.GetComponent<NavMeshObstacle>().enabled = false;
        holdItem = ball.GetComponent<Interactable>();
    }
    public void ReleaseBall(Transform ball)
    {
        holdItem = null;
    }
    #endregion

    #region legoBox
    public void GrabLegoBox(GameObject Lego)
    {
        Lego.gameObject.transform.parent = handPivot;
        Lego.transform.localPosition = Vector3.zero;
        holdItem = Lego.GetComponent<Interactable>();
    }

    public void DropLego(Interactable Lego)
    {
        holdItem = null;
    }
    #endregion

    public void TryGrab()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.localPosition, 0.2f, transform.forward, 0.55f, LayerMask.GetMask("Items"));
        //debug all hits
        Debug.Log(hits.Length);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent<Interactable>(out Interactable inter))
            {
                inter.OnInteract(this);
                return;
            }
        }
    }

}
