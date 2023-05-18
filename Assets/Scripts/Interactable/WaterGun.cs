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
    private float currentAmmo;

    public float CurrentAmmo
    {
        get => currentAmmo;
        set
        {
            currentAmmo = value;
            GameManager.Instance.UiManager.GetPlayerUI(currentHolder.PlayerNumber).UpdateWaterUI(currentAmmo / maxAmmo);
        }
    }

    public override void Start()
    {
        base.Start();
        ps = GetComponentInChildren<ParticleSystem>();
        waterCollider = GetComponent<Collider>();
        startPosition = transform.position;
        waterParts = GetComponentInChildren<WaterParts>();
        currentAmmo = maxAmmo;
    }

    public void FixedUpdate()
    {
        if (isActive)
        {
            CurrentAmmo -= Time.fixedDeltaTime;
            if (CurrentAmmo <= 0)
            {
                ps.Stop();
                isActive = false;
            }
        }
        if (currentHolder != null)
        {
            transform.rotation = Quaternion.LookRotation(-Vector3.up, Quaternion.Euler(0, -90, 0) * currentHolder.Forward);
        }
    }

    public void Reload()
    {
        CurrentAmmo = maxAmmo;
    }

    public override void Dropped(PlayerController player)
    {
        base.Dropped(player);
        transform.parent = Interactables;
        transform.position = startPosition;
        waterCollider.enabled = true;
        player.DropWaterGun();
        waterParts.player = null;
        ps.Stop();
        isActive = false;
        currentHolder = null;
        GameManager.Instance.UiManager.GetPlayerUI(player.PlayerNumber).ActivateWaterSlider(false);
    }

    public override void OnCollide(PlayerController player)
    {
        if (!player.hasFreeHands) return;
        player.GrabWaterGun(transform);
        waterCollider.enabled = false;
        waterParts.player = player;
        currentHolder = player;

        GameManager.Instance.UiManager.GetPlayerUI(player.PlayerNumber).ActivateWaterSlider(true);
        GameManager.Instance.UiManager.GetPlayerUI(currentHolder.PlayerNumber).UpdateWaterUI(currentAmmo / maxAmmo);
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
            if (CurrentAmmo > 0)
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
