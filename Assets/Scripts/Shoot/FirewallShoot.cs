using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallShoot : Shoot
{
    public GameObject bulletPrefab;
    public Vector3 offset;

    public int fanAngle = 50;
    public float coolDownAttack = 1f;
    public float coolDownBullet = 0.1f;

    public AudioClip shootSound;
    private AudioSource audioSource;

    private float timeAfterLastShoot;
    private float angel;
    private float back;

    private void Start()
    {
        angel = fanAngle / 2;
        back = fanAngle / 2;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShoot += Time.deltaTime;
        if (timeAfterLastShoot >= coolDownAttack)
        {
            audioSource.PlayOneShot(shootSound);
            Vector3 eulerAngles = transform.eulerAngles;
            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset,
                Quaternion.Euler(eulerAngles.x, eulerAngles.y - back + angel * 0, eulerAngles.z), coolDownBullet * 0));
            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset, 
                Quaternion.Euler(eulerAngles.x, eulerAngles.y - back + angel * 1, eulerAngles.z), coolDownBullet * 1));
            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset, 
                Quaternion.Euler(eulerAngles.x, eulerAngles.y - back + angel * 2, eulerAngles.z), coolDownBullet*2));
            timeAfterLastShoot = 0f;
        }
    }
}