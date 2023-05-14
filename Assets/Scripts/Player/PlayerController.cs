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
    public Transform StartPoint;

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
    public Vector3 Forward;

    public float wetness;
    public float maxWetness = 30;

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
        GameManager.Instance.RegisterPlayer(PlayerNumber, this);
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
        if (wetness > 0) wetness -= Time.fixedDeltaTime;
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
        else
        {
            float wetRation = 1 - (wetness / maxWetness);
            agent.Move(moveDirection.normalized * MoveSpeed * wetRation);
        }

        if (moveDirection != Vector3.zero) Forward = moveDirection.normalized;
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

    public void ResetChild()
    {
        KnockOut(null, 5);
        agent.Warp(StartPoint.position);
    }

    public void KnockOut(Transform projectil, float duration)
    {
        Debug.Log("knock out " + gameObject.name, gameObject);
        agent.enabled = false;
        isKnockedOut = true;
        knockOutDelay = duration;
        knockOutTimer = 0;
        wetness = 0;
        if (holdItem != null) holdItem.Dropped(this);
    }

    public void DropItem()
    {
        if (holdItem != null) holdItem.Dropped(this);
    }

    #region tablette
    public void GrabTablette(Transform tablette)
    {
        tablette.gameObject.transform.parent = handPivot;
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

    #region NerfGun

    public void GrabNerfGun(Transform nerfGun)
    {
        nerfGun.gameObject.transform.parent = handPivot;
        nerfGun.transform.localPosition = Vector3.zero;
        holdItem = nerfGun.GetComponent<Interactable>();
    }

    public void DropNerfGun()
    {
        holdItem = null;
    }

    public void ReloadNerfGun(GameObject ammo)
    {
        if (holdItem == null) return;
        if (holdItem is NerfGun)
        {
            NerfGun nerf = holdItem as NerfGun;
            nerf.Ammo++;
            Destroy(ammo);
        }
    }

    #endregion

    #region WaterGun
    public void GrabWaterGun(Transform waterGun)
    {
        waterGun.gameObject.transform.parent = handPivot;
        waterGun.transform.localPosition = Vector3.zero;
        holdItem = waterGun.GetComponent<Interactable>();
    }

    public void DropWaterGun()
    {
        holdItem = null;
    }

    public void ReloadWaterGun()
    {
        if (holdItem == null) return;
        if (holdItem is WaterGun)
        {
            WaterGun water = holdItem as WaterGun;
            water.Reload();
        }
    }

    public void Wet()
    {
        if (isKnockedOut) return;
        wetness++;
        if (wetness > maxWetness)
        {
            wetness = 0;
            KnockOut(null, 1f);
        }
    }

    #endregion
    public void TryGrab()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.localPosition, 0.5f, Forward, 1f, LayerMask.GetMask("Items"));
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
