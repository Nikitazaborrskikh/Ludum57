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

    private float timeAfterLastShot;

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShot += Time.deltaTime;    
        if (Input.GetButtonDown("Fire1") && timeAfterLastShot >= coolDownStandartAttack)
        {
            BulletManager.Instance.ShootBullet(bulletStandartPrefab, gameObject, transform.position + transform.rotation * offset);
            timeAfterLastShot = 0f;
        }
        if (Input.GetButtonDown("Fire2") && timeAfterLastShot >= coolDownLargeAttack)
        {
            BulletManager.Instance.ShootBullet(bulletLargePrefab, gameObject, transform.position + transform.rotation * offset);
            timeAfterLastShot = 0f;
        }
    }
}