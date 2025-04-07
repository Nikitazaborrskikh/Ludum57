using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoFactorAuthenticationShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Vector3 offset;

    public float coolDownAttack = 1f;
    public int countBullets = 6;

    private float timeAfterLastShot;

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShot += Time.deltaTime;
        if (timeAfterLastShot >= coolDownAttack)
        {
            for (int i = 0; i < countBullets; i++)
            {
                StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset, Quaternion.Euler(0f, 360/countBullets*i, 0f), 0.1f));
            }
            timeAfterLastShot = 0f;
        }
    }
}
