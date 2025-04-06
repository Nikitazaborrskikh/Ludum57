using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapthaShoot : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб пули
    public Vector3 offset;

    public float coolDownAttack = 1f;

    private float timeAfterLastShot;

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShot += Time.deltaTime;
        if (timeAfterLastShot >= coolDownAttack)
        {
            BulletManager.Instance.ShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset);
            timeAfterLastShot = 0f;
        }
    }
}