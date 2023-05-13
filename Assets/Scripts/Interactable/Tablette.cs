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

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
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
    }


    public override void OnCollide(PlayerController player)
    {
        if (justGrabbed) return;
        if (!player.hasFreeHands) return;
        if (player.isKnockedOut) return;
        if (currentHolder != null) currentHolder.DropTablette();
        player.GrabTablette(transform);
        currentHolder = player;
        justGrabbed = true;

    }

    public override void OnInteract(PlayerController player)
    {

    }

    public override void Dropped(PlayerController player)
    {
        transform.parent = Interactables;
        player.DropTablette();
        agent.Move(player.transform.forward);
    }

    public override void Use(PlayerController player)
    {
        player.StartDash();
    }
}
