using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class Bullet : MonoBehaviour
{
    public string type; // "Standart"/"Large"
    public float speed; // �������� ����
    public float lifetime = 2f; // ����� ����� ����
    private float timeAlive;

    private void OnEnable()
    {
        timeAlive = 0f; // ����� ������� ����� ��� ���������
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
            gameObject.SetActive(false); // ������������ ���� ����� ��������� ������� �����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("��������� � �����: " + other.gameObject.name);
            gameObject.SetActive(false);
        }
    }

    private void OnRenderObject()
    {
    }
}