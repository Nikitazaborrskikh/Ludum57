using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class UserPageShoot : MonoBehaviour
{
    public GameObject bulletPrefab1; // Префаб пули
    public GameObject bulletPrefab2; // Префаб пули
    public Vector3 offset1;
    public Vector3 offset2;

    public float coolDownAttack = 1f;

    private float timeAfterLastShoot;

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShoot += Time.deltaTime;
        if (timeAfterLastShoot >= coolDownAttack)
        {
            StartCoroutine(RunTwoFunctions());
            timeAfterLastShoot = 0f;
        }
    }

    private IEnumerator RunTwoFunctions()
    {
        // Запускаем обе функции как корутины
        yield return BulletManager.Instance.CallShootBullet(bulletPrefab1, gameObject, transform.position + transform.rotation * offset1, transform.rotation, 0.1f);
        yield return BulletManager.Instance.CallShootBullet(bulletPrefab2, gameObject, transform.position + transform.rotation * offset2, transform.rotation, 0.0f);
    }   
}