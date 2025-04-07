using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class UserPageShoot : Shoot
{
    public GameObject bulletPrefab; // Префаб пули
    public Vector3 offset1;
    public Vector3 offset2;

    public float coolDownAttack = 1f;

    public AudioClip shootSound;
    private AudioSource audioSource;

    private float timeAfterLastShoot;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShoot += Time.deltaTime;
        if (timeAfterLastShoot >= coolDownAttack)
        {
            audioSource.PlayOneShot(shootSound);
            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset1, transform.rotation, 0.1f));
            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset2, transform.rotation, 0.1f));
            timeAfterLastShoot = 0f;
        }
    }
}