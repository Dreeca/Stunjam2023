using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NoiseManager : MonoBehaviour
{
    public float MaxNoise;
    public float currentNoise;
    public NavMeshAgent Parent;
    public List<NoiseStep> steps;
    public int currentStep;
    public GameObject zone;

    public Transform Stairs;
    public Transform Bed;

    public Transform room1;
    public Transform room2;
    public Transform room3;
    public Transform room4;

    public Transform Tablette;
    public Transform DropPoint;

    [Header("Noise type")]
    public float BallNoise;
    public float NerfHit;
    public float WetHit;
    public float LegoHit;

    [SerializeField]
    private bool lookingAround;
    [SerializeField]
    private int lookAroundStep;

    [SerializeField]
    private bool lookingForTablette;
    [SerializeField]
    private bool hasTablette;
    [SerializeField]
    private bool listening;
    [SerializeField]
    private bool goingToBed;

    [SerializeField]
    private bool isAwake;
    public bool IsAwake
    {
        get => isAwake;
        set
        {
            isAwake = value;
            Parent.gameObject.SetActive(isAwake);

        }
    }

    [System.Serializable]
    public class NoiseStep
    {
        public float value;
        public UnityEvent Call;
    }

    public void FixedUpdate()
    {
        if (IsAwake)
        {
            GameManager.Instance.ActivePlayer.ForEach(x =>
            {
                if (Vector3.Distance(Parent.transform.position, x.transform.position) < zone.transform.localScale.x / 2f)
                {
                    x.ResetChild();
                }
            });
        }

        if (Parent.hasPath && goingToBed && Parent.remainingDistance < 0.1f)
        {
            goingToBed = false;
            IsAwake = false;
        }

        if (lookingAround)
        {
            if (Parent.remainingDistance < 1f)
            {
                switch (lookAroundStep)
                {
                    case 0:
                        Parent.SetDestination(room2.position);
                        lookAroundStep++;
                        break;
                    case 1:
                        Parent.SetDestination(room3.position);
                        lookAroundStep++;
                        break;
                    case 2:
                        Parent.SetDestination(room4.position);
                        lookAroundStep++;
                        break;
                    case 3:
                        Parent.SetDestination(Bed.position);
                        lookingAround = false;
                        goingToBed = true;
                        break;
                }
            }
        }

        if (lookingForTablette)
        {
            Parent.SetDestination(Tablette.position);
        }
        if (hasTablette && Parent.remainingDistance < 0.25f)
        {
            Tablette.parent = Tablette.GetComponent<Interactable>().Interactables;
            Tablette.transform.position = DropPoint.position;
            Tablette.GetComponent<NavMeshAgent>().enabled = true;
            listening = true;
        }
        if (lookingForTablette && Vector3.Distance(Parent.transform.position, Tablette.position) < 0.75f)
        {
            Tablette.parent = Parent.transform;
            Tablette.localPosition = Vector3.zero;
            Tablette.GetComponent<NavMeshAgent>().enabled = false;
            Parent.SetDestination(Bed.position);
            hasTablette = true;
            lookingForTablette = false;
        }
    }

    public void MakeNoise(Vector3 position, float value)
    {
        currentNoise += value;
        if (currentStep < steps.Count && ((currentNoise / MaxNoise) > steps[currentStep].value))
        {
            steps[currentStep].Call.Invoke();
            currentStep++;
        }

        GameManager.Instance.UiManager.Noise.value = currentNoise / MaxNoise;
        if (listening && Parent.remainingDistance < 1f)
        {
            Parent.SetDestination(position);
        }
    }

    public void GoToBed()
    {
        //Debug.Log("go to bed");
        IsAwake = true;
        Parent.gameObject.SetActive(true);
        Parent.enabled = true;
        Parent.speed = 2f;
        Parent.SetDestination(Bed.position);
        goingToBed = true;
    }

    public void StartLookAround()
    {
        //Debug.Log("look around");
        lookingAround = true;
        IsAwake = true;
        Parent.speed = 4f;
        Parent.SetDestination(room1.position);
        goingToBed = false;
    }

    public void StartLookingForTablette()
    {
        //Debug.Log("look for tablette");
        lookingForTablette = true;
        IsAwake = true;
        Parent.speed = 6f;
        Parent.SetDestination(Tablette.position);
        goingToBed = false;
        lookingAround = false;
    }

}
