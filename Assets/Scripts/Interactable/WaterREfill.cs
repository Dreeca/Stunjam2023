using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterREfill : Interactable
{
    public override void Dropped(PlayerController player)
    {
    }

    public override void OnCollide(PlayerController player)
    {
        player.ReloadWaterGun();
    }

    public override void OnInteract(PlayerController player)
    {
    }

    public override void Use(PlayerController player)
    {
    }
}
