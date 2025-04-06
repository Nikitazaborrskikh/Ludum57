using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapthaShoot : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб пули
    public Vector3 offset;

    public float coolDownAttack = 1f;

    private float timeAfterLastShoot;

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShoot += Time.deltaTime;
        if (timeAfterLastShoot >= coolDownAttack)
        {
            BulletManager.Instance.ShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset, transform.rotation);
            timeAfterLastShoot = 0f;
        }
    }
}