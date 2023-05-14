using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGun : Interactable
{
    private ParticleSystem ps;
    private bool isActive = false;
    private Collider waterCollider;
    private Vector3 startPosition;
    private WaterParts waterParts;
    public float maxAmmo = 10;
    public float currentAmmo = 10;
    private PlayerController controller;



    public override void Start()
    {
        base.Start();
        ps = GetComponentInChildren<ParticleSystem>();
        waterCollider = GetComponent<Collider>();
        startPosition = transform.position;
        waterParts = GetComponentInChildren<WaterParts>();
    }

    public void FixedUpdate()
    {
        if (isActive)
        {
            currentAmmo -= Time.fixedDeltaTime;
            if (currentAmmo <= 0)
            {
                ps.Stop();
                isActive = false;
            }
        }
        if (controller != null)
        {
            transform.rotation = Quaternion.LookRotation(-Vector3.up, Quaternion.Euler(0, -90, 0) * controller.Forward);
        }
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
    }

    public override void Dropped(PlayerController player)
    {
        transform.parent = Interactables;
        transform.position = startPosition;
        waterCollider.enabled = true;
        player.DropWaterGun();
        waterParts.player = null;
        ps.Stop();
        isActive = false;
        controller = null;
    }

    public override void OnCollide(PlayerController player)
    {
        if (!player.hasFreeHands) return;
        player.GrabWaterGun(transform);
        waterCollider.enabled = false;
        waterParts.player = player;
        controller = player;
    }

    public override void OnInteract(PlayerController player)
    {
    }

    public override void Use(PlayerController player)
    {
        if (isActive)
        {
            ps.Stop();
            isActive = false;
        }
        else
        {
            if (currentAmmo > 0)
            {
                ps.Play();
                isActive = true;
            }
            else
            {
                Dropped(player);
            }
        }
    }
}
