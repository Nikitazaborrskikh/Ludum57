using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackupShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Vector3 offset;

    public float coolDownAttack = 1f;
    public float coolDownBullet = 0.1f;
    public int countBullets = 6;
    public int fanAngle = 180;

    private float timeAfterLastShot;
    private float angel;
    private float back;

    private void Start()
    {
        angel = fanAngle / (countBullets-1);
        back = fanAngle / 2;
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShot += Time.deltaTime;
        if (timeAfterLastShot >= coolDownAttack)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            for (int i = 0; i < countBullets; i++)
            {
                StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab, gameObject, transform.position + transform.rotation * offset,
                Quaternion.Euler(eulerAngles.x, eulerAngles.y - back + angel * i, eulerAngles.z), coolDownBullet*i));
                timeAfterLastShot = 0f;
            }
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TwoFactorAuthenticationShoot : MonoBehaviour
//{
//    public GameObject bulletPrefab1;
//    public GameObject bulletPrefab2;
//    public Vector3 offset;

//    public float coolDownAttack = 1f;
//    public float coolDownBullet = 0.1f;

//    private float timeAfterLastShot;

//    // Update is called once per frame
//    void Update()
//    {
//        timeAfterLastShot += Time.deltaTime;
//        if (timeAfterLastShot >= coolDownAttack)
//        {
//            BulletManager.Instance.ShootBullet(bulletPrefab1, gameObject, transform.position + transform.rotation * offset);
//            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab2, gameObject, transform.position + transform.rotation * offset, coolDownBullet));
//            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab1, gameObject, transform.position + transform.rotation * offset, coolDownBullet * 2));
//            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab1, gameObject, transform.position + transform.rotation * offset, coolDownBullet * 3));
//            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab1, gameObject, transform.position + transform.rotation * offset, coolDownBullet * 4));
//            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab1, gameObject, transform.position + transform.rotation * offset, coolDownBullet * 5));
//            timeAfterLastShot = 0f;
//        }
//    }
//}