using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 10;
    public AudioClip damageSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void TakeDamage(int damage)
    {
        audioSource.PlayOneShot(damageSound);
        health -= damage;
        if (health <= 0)
        {
            death();
        }
        Debug.Log($"Player health: {health}");
    }

    private void death()
    {
        audioSource.PlayOneShot(deathSound);
    }
}

