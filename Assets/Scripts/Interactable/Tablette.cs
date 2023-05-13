using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablette : Interactable
{

    public float delay = 0.5f;
    private bool justGrabbed;
    private float delayTimer;
    private PlayerController currentHolder;

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
    }
}
