using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapthaShoot : Shoot
{
    public GameObject bulletPrefab; // ������ ����
    public Vector3 offset;

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
            BulletManager.Instance.ShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset, transform.rotation);
            timeAfterLastShoot = 0f;
        }
    }
}