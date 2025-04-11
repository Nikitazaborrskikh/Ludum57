using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Zenject.SpaceFighter;
using static UnityEngine.Rendering.DebugUI;

public class Bullet : MonoBehaviour
{
    private float damage;
    private bool canRicochet;
    private bool canSniff;

    public void SetDamage(float dmg) => damage = dmg;
    public void EnableRicochet() => canRicochet = true;
    public void EnableSniffing() => canSniff = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(collision.gameObject.name + " collided with " + collision.gameObject.name);
            BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
            enemy.TakeDamage(damage);
            if (canRicochet)
            {
                GameObject nearestEnemy = FindNearestEnemy(collision.transform.position);
                if (nearestEnemy)
                {
                    Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
                    GetComponent<Rigidbody>().velocity = direction; 
                    nearestEnemy.GetComponent<BaseEnemy>().TakeDamage(damage);
                    canRicochet = false;
                    Destroy(gameObject);
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
        else if (collision.gameObject.CompareTag("PlayerBullet") || collision.gameObject.CompareTag("Player"))
        {
            
            return;
        }
        else
        {
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