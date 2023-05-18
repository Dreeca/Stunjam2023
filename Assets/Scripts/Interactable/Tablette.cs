using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tablette : Interactable
{

    public float delay = 0.5f;
    private bool justGrabbed;
    private float delayTimer;
    private NavMeshAgent agent;
    public float DashDelay = 1f;
    private float dashTimer;
    private bool hasDashed = false;

    public float DashTimer
    {
        get => dashTimer;
        set
        {
            dashTimer = value;
            if (currentHolder != null) GameManager.Instance.UiManager.GetPlayerUI(currentHolder.PlayerNumber).UpdateDashUI(dashTimer / DashDelay);
        }
    }

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        dashTimer = DashDelay;
    }


    public void FixedUpdate()
    {
        if (justGrabbed)
        {
            delayTimer += Time.fixedDeltaTime;
            if (delayTimer > delay)
            {
                justGrabbed = false;
                delayTimer = 0;
            }
        }
        if (hasDashed)
        {
            DashTimer += Time.fixedDeltaTime;
            if (DashTimer > DashDelay)
            {
                hasDashed = false;
            }
        }
    }


    public override void OnCollide(PlayerController player)
    {
        if (justGrabbed) return;
        if (!player.hasFreeHands) player.DropItem();
        if (player.isKnockedOut) return;
        if (currentHolder != null) currentHolder.DropTablette();
        player.GrabTablette(transform);
        currentHolder = player;
        justGrabbed = true;
        agent.enabled = false;
        GameManager.Instance.UiManager.GetPlayerUI(player.PlayerNumber).ActivateDashSlider(true);
        GameManager.Instance.UiManager.GetPlayerUI(currentHolder.PlayerNumber).UpdateDashUI(dashTimer / DashDelay);
    }

    public override void OnInteract(PlayerController player)
    {

    }

    public override void Dropped(PlayerController player)
    {
        base.Dropped(player);
        transform.parent = Interactables;
        player.DropTablette();
        agent.enabled = true;
        agent.Move(player.Forward);
        currentHolder = null;
        GameManager.Instance.UiManager.GetPlayerUI(player.PlayerNumber).ActivateDashSlider(false);
    }

    public override void Use(PlayerController player)
    {
        if (hasDashed) return;
        player.StartDash();
        hasDashed = true;
        DashTimer = 0;
    }
}
