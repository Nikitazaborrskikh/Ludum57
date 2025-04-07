using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    public GameObject pullBullet;
    public static BulletManager Instance { get; private set; }

    private List<Bullet> bullets = new List<Bullet>(); // ������ �������� ����

    private void Awake()
    {
        // ���������, ���������� �� ��� ��������� BulletManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ���������� ��������
            return;
        }

        Instance = this; // ������������� ������� ��������� ��� ������������
        DontDestroyOnLoad(gameObject); // �� ���������� ������ ��� �������� ����� �����
    }

    private void CreateBullet(GameObject bullet)
    {
        GameObject newBullet = Instantiate(bullet, pullBullet.transform);
        newBullet.SetActive(false);
        bullets.Add(newBullet.GetComponent<Bullet>());
    }

    private Bullet findFreeBullet(GameObject bulletPrefab)
    {
        foreach (var bullet in bullets)
        {
            if ((!bullet.gameObject.activeInHierarchy) && bullet.gameObject.name == bulletPrefab.name + "(Clone)")
            {
                return bullet;
            }
        }
        return null;
    }

    public IEnumerator CallShootBullet(GameObject bulletPrefab, GameObject shooter, Vector3 newPositiion, Quaternion forward, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        ShootBullet(bulletPrefab, shooter, newPositiion, forward);
    }

    private void activateBullet(GameObject bulletPrefab, GameObject shooter, Vector3 newPositiion, Quaternion forward)
    {
        var bullet = findFreeBullet(bulletPrefab);
        bullet.transform.position = newPositiion;
        bullet.transform.rotation = forward;
       // bullet.shooter = shooter;
        bullet.gameObject.SetActive(true);
    }
    public void ShootBullet(GameObject bulletPrefab, GameObject shooter, Vector3 newPositiion, Quaternion forward)
    {
        if (findFreeBullet(bulletPrefab)) { activateBullet(bulletPrefab, shooter, newPositiion, forward); return; }
        CreateBullet(bulletPrefab);
        activateBullet(bulletPrefab, shooter, newPositiion, forward);
    }

    private void OnRenderObject()
    {
        // ������ ��� �������� ����
        foreach (var bullet in bullets)
        {
            if (bullet.gameObject.activeInHierarchy)
            {
                Graphics.DrawMesh(bullet.GetComponent<MeshFilter>().sharedMesh, bullet.transform.position, bullet.transform.rotation, bullet.GetComponent<Renderer>().sharedMaterial, 0);
            }
        }
    }
}