using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletStandartPrefab; // Префаб пули
    public GameObject bulletLargePrefab; // Префаб пули
    public Transform firePoint; // Точка, откуда будет вылетать пуля
    public int maxBullets = 20; // Максимальное количество пуль
    private List<Bullet> bullets = new List<Bullet>(); // Список активных пуль

    private void Start()
    {
        // Создаем пул пуль
        for (int i = 0; i < maxBullets/2; i++)
        {
            GameObject newBullet = Instantiate(bulletStandartPrefab);
            newBullet.SetActive(false);
            bullets.Add(newBullet.GetComponent<Bullet>());
        }

        for (int i = 0; i < maxBullets/2; i++)
        {
            GameObject newBullet = Instantiate(bulletLargePrefab);
            newBullet.SetActive(false);
            bullets.Add(newBullet.GetComponent<Bullet>());
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet("Standart");
        }
        if (Input.GetButtonDown("Fire2"))
        {
            ShootBullet("Large");
        }
    }


    void ShootBullet(string type)
    {
        foreach (var bullet in bullets)
        {
            if (!bullet.gameObject.activeInHierarchy && bullet.type == type)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                bullet.gameObject.SetActive(true); 
                return; 
            }
        }
    }

    private void OnRenderObject()
    {
        // Рисуем все активные пули
        foreach (var bullet in bullets)
        {
            if (bullet.gameObject.activeInHierarchy)
            {
                Graphics.DrawMesh(bullet.GetComponent<MeshFilter>().sharedMesh, bullet.transform.position, bullet.transform.rotation, bullet.GetComponent<Renderer>().sharedMaterial, 0);
            }
        }
    }
}