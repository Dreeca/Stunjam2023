using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParts : MonoBehaviour
{
    private ParticleSystem ps;
    private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
    public PlayerController player;

    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void OnParticleTrigger()
    {
        if (player == null) return;
        ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles, out ParticleSystem.ColliderData collider);

        for (int p = 0; p < particles.Count; p++)
        {
            for (int i = 0; i < collider.GetColliderCount(0); i++)
            {
                Transform col = collider.GetCollider(0, i).transform;
                if (col != player.transform && col.CompareTag("Player"))
                {
                    col.GetComponent<PlayerController>().Wet();
                }
            }
        }
    }

}
