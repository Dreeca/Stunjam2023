using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Transform Interactables;
    protected PlayerController currentHolder;
    public virtual void Start()
    {
        if (Interactables == null) Interactables = transform.parent;
    }

    public abstract void OnInteract(PlayerController player);

    public abstract void OnCollide(PlayerController player);

    public abstract void Dropped(PlayerController player);

    public abstract void Use(PlayerController player);

}
