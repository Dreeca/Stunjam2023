using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerfGun : Interactable
{
    public GameObject NerfBullet;
    public Vector3 offset = new Vector3(0, 1, 0);
    public float Duration = 1f;
    private Vector3 startPosition;
    private Collider gunCollider;
    private LineRenderer lineRenderer;

    public float rayDuration = 0.25f;
    private float rayTimer;

    public int Ammo = 3;

    public override void Start()
    {
        base.Start();
        gunCollider = GetComponent<Collider>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.enabled = false;
        startPosition = transform.position;
    }

    public void FixedUpdate()
    {
        if (lineRenderer.enabled)
        {
            rayTimer += Time.fixedDeltaTime;
            if (rayTimer > rayDuration) lineRenderer.enabled = false;
        }
    }

    public override void Dropped(PlayerController player)
    {
        transform.parent = Interactables;
        transform.position = startPosition;
        gunCollider.enabled = true;
        player.DropNerfGun();
    }

    public override void OnCollide(PlayerController player)
    {
        if (!player.hasFreeHands) return;
        player.GrabNerfGun(transform);
        gunCollider.enabled = false;
    }

    public override void OnInteract(PlayerController player)
    {

    }

    public override void Use(PlayerController player)
    {
        if (Ammo <= 0)
        {
            Dropped(player);
            return;
        }
        if (Physics.Raycast(player.transform.position + offset, player.Forward, out RaycastHit hit, 100f))
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position + offset);
            lineRenderer.SetPosition(1, hit.point + offset);
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerController>().KnockOut(transform, Duration);
            }
            GameObject ammo = Instantiate(NerfBullet, hit.point, Quaternion.identity);
            GameManager.Instance.Noise.MakeNoise(hit.point, GameManager.Instance.Noise.NerfHit);
        }
        else
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position + offset);
            lineRenderer.SetPosition(1, transform.position + offset + player.Forward * 100);
        }
        rayTimer = 0;
        Ammo--;

    }
}
