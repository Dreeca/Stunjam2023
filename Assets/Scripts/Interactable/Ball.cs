using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Interactable
{
    public float throwForce = 10f;
    public float knockOutTime = 2f;
    private Rigidbody body;

    private PlayerController previousHolder;

    private bool knocks;
    private float previousMagnitude;

    public override void Start()
    {
        base.Start();
        body = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        if (knocks)
        {
            float currentMagnitude = body.velocity.magnitude;
            Debug.Log(body.velocity.magnitude);
            if (previousMagnitude < 2f && currentMagnitude < 2f)
            {
                knocks = false;
            }
            previousMagnitude = currentMagnitude;
        }
    }

    public override void Dropped(PlayerController player)
    {
        Use(player);
    }

    public override void OnCollide(PlayerController player)
    {
        if (player == previousHolder) return;
        if (knocks)
        {
            player.KnockOut(transform, knockOutTime);
            knocks = false;
        }
    }

    public override void OnInteract(PlayerController player)
    {
        player.GrabBall(transform);
        knocks = false;
        currentHolder = player;
    }

    public override void Use(PlayerController player)
    {
        transform.parent = Interactables.transform;
        Vector3 ballPosition = (player.transform.position + player.transform.forward * 1);
        ballPosition.y = 0.7f;
        transform.position = ballPosition;
        body.isKinematic = false;
        body.AddForce(player.transform.forward * throwForce, ForceMode.VelocityChange);

        currentHolder.ReleaseBall(transform);
        currentHolder = null;

        knocks = true;
        previousMagnitude = float.MaxValue;
    }
}
