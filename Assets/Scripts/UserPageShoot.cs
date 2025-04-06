using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class UserPageShoot : MonoBehaviour
{
    public GameObject bulletPrefab1; // ������ ����
    public GameObject bulletPrefab2; // ������ ����
    public Vector3 offset1;
    public Vector3 offset2;

    public float coolDownAttack = 1f;

    private float timeAfterLastShot;

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShot += Time.deltaTime;
        if (timeAfterLastShot >= coolDownAttack)
        {
            StartCoroutine(RunTwoFunctions());
            timeAfterLastShot = 0f;
        }
    }

    private IEnumerator RunTwoFunctions()
    {
        // ��������� ��� ������� ��� ��������
        yield return BulletManager.Instance.CallShootBullet(bulletPrefab1, gameObject, transform.position + transform.rotation * offset1, 0.1f);
        yield return BulletManager.Instance.CallShootBullet(bulletPrefab2, gameObject, transform.position + transform.rotation * offset2, 0.0f);
    }   
}