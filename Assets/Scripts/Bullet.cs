using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class Bullet : MonoBehaviour
{
    public string type; // "Standart"/"Large"
    public float speed; // Скорость пули
    public float lifetime = 2f; // Время жизни пули
    private float timeAlive;

    private void OnEnable()
    {
        timeAlive = 0f; // Сброс времени жизни при активации
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive < lifetime)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            gameObject.SetActive(false); // Деактивируем пулю после истечения времени жизни
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Попадание в врага: " + other.gameObject.name);
            gameObject.SetActive(false);
        }
    }

    private void OnRenderObject()
    {
    }
}