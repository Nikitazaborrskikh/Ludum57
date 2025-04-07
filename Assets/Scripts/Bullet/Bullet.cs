using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Zenject.SpaceFighter;

public class Bullet : MonoBehaviour
{
    private float damage;
    private bool canRicochet;
    private bool canSniff;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log($"Bullet Awake - Initial Velocity: {rb.velocity}, UseGravity: {rb.useGravity}");
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            Debug.Log($"Bullet FixedUpdate - Current Velocity: {rb.velocity}");
        }
    }

    public void SetDamage(float dmg) => damage = dmg;
    public void EnableRicochet() => canRicochet = true;
    public void EnableSniffing() => canSniff = true;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Bullet collided with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
            enemy.TakeDamage(damage);
            if (canRicochet)
            {
                GameObject nearestEnemy = FindNearestEnemy(collision.transform.position);
                if (nearestEnemy)
                {
                    Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
                    rb.velocity = direction * 10f;
                    nearestEnemy.GetComponent<BaseEnemy>().TakeDamage(damage);
                    canRicochet = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (canSniff && collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private GameObject FindNearestEnemy(Vector3 position)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, position);
            if (dist < minDist && dist > 0.1f)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }
}