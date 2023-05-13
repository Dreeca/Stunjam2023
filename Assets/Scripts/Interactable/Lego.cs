using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lego : Interactable
{

    private PlayerController user;
    public float Duration;

    public override void Dropped(PlayerController player)
    {
        Use(player);
    }

    public override void OnCollide(PlayerController player)
    {
        if (player != user) { player.KnockOut(transform, Duration); Destroy(this.gameObject); }
    }

    public override void OnInteract(PlayerController player)
    {
    }

    public override void Use(PlayerController player)
    {
        transform.parent = Interactables;
        Vector3 pos = player.transform.position + player.transform.forward;
        pos.y = 0.6f;
        transform.position = pos;
        user = player;
        player.DropLego(this);
    }

}
