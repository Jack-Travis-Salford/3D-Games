using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [field: SerializeField] private Collider playerCollider;
    [field: SerializeField] private AudioClip hitClip;
 
    private int damage;

    private List<Collider> collidedWithPlayers = new List<Collider>();
    private List<Collider> collidedWithShields = new List<Collider>();

    private void OnEnable()
    {
        collidedWithPlayers.Clear();
        collidedWithShields.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider) { return; }

        if (playerCollider.CompareTag(other.tag))
        {
            return;
        }

        if (collidedWithShields.Contains(other)) {return; }
        if(collidedWithPlayers.Contains(other)) { return; }
        
        if (other.CompareTag("Shield"))
        {
            if (other.TryGetComponent<Collider>(out Collider shieldCollider))
            {
                collidedWithShields.Add(shieldCollider);
            }

            AudioSource shieldAudioSource = other.GetComponentInParent<AudioSource>();
            shieldAudioSource.Play();
            
            return;
        }

        
        if (other.TryGetComponent<Health>(out Health health))
        {
            if (collidedWithShields.Contains(health.ShieldCollider)) { return; }
            health.ApplyDamage(damage);
            collidedWithPlayers.Add(other);
             if(other.TryGetComponent<AudioSource>(out AudioSource audioSource))
            {
                audioSource.PlayOneShot(hitClip);
            } 
        }
      
        
    }

    public void IncreaseDamage()
    {
        damage += 10;
    }
    public void SetAttack(int damage)
    {
        this.damage = damage;
    }
}
