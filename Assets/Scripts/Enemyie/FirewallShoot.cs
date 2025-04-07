using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallShoot : MonoBehaviour
{
    public GameObject bulletPrefab1;
    public GameObject bulletPrefab2;
    public Vector3 offset;

    public int fanAngle = 50;
    public float coolDownAttack = 1f;
    public float coolDownBullet = 0.1f;

    private float timeAfterLastShoot;
    private float angel;
    private float back;

    private void Start()
    {
        angel = fanAngle / 2;
        back = fanAngle / 2;
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShoot += Time.deltaTime;
        if (timeAfterLastShoot >= coolDownAttack)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            BulletManager.Instance.ShootBullet(bulletPrefab1, gameObject, transform.position + transform.rotation * offset,
                Quaternion.Euler(eulerAngles.x, eulerAngles.y - back + angel * 0, eulerAngles.z));
            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab2, gameObject, transform.position + transform.rotation * offset, 
                Quaternion.Euler(eulerAngles.x, eulerAngles.y - back + angel * 1, eulerAngles.z), coolDownBullet));
            StartCoroutine(BulletManager.Instance.CallShootBullet(bulletPrefab1, gameObject, transform.position + transform.rotation * offset, 
                Quaternion.Euler(eulerAngles.x, eulerAngles.y - back + angel * 2, eulerAngles.z), coolDownBullet*2));
            timeAfterLastShoot = 0f;
        }
    }
}