using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletStandartPrefab; // Префаб пули
    public GameObject bulletLargePrefab; // Префаб пули
    public Vector3 offset;

    public float coolDownStandartAttack = 1f;
    public float coolDownLargeAttack = 1.5f;

    private float timeAfterLastShoot;

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShoot += Time.deltaTime;    
        if (Input.GetButtonDown("Fire1") && timeAfterLastShoot >= coolDownStandartAttack)
        {
            BulletManager.Instance.ShootBullet(bulletStandartPrefab, gameObject, transform.position + transform.rotation * offset, transform.rotation);
            timeAfterLastShoot = 0f;
        }
        if (Input.GetButtonDown("Fire2") && timeAfterLastShoot >= coolDownLargeAttack)
        {
            BulletManager.Instance.ShootBullet(bulletLargePrefab, gameObject, transform.position + transform.rotation * offset, transform.rotation);
            timeAfterLastShoot = 0f;
        }
    }
}