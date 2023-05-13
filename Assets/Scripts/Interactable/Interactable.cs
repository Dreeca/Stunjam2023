using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
  
    public abstract void OnInteract(PlayerController player);

    public abstract void OnCollide(PlayerController player);

    public abstract void Dropped(PlayerController player);

}
