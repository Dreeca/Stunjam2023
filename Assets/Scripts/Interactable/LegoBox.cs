using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoBox : Interactable
{

    public GameObject LegoAsset;

    public override void Dropped(PlayerController player)
    {
        base.Dropped(player);
    }

    public override void OnCollide(PlayerController player)
    {
    }

    public override void OnInteract(PlayerController player)
    {
        GameObject LegoInstance = GameObject.Instantiate(LegoAsset);
        LegoInstance.GetComponent<Interactable>().Interactables = Interactables;
        player.GrabLegoBox(LegoInstance);
    }

    public override void Use(PlayerController player)
    {
    }
}
