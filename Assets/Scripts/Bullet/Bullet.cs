using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;
using static UnityEngine.Rendering.DebugUI;

public class Bullet : MonoBehaviour
{
    public float speed; // Скорость пули
    public float lifetime = 2f; // Время жизни пули
    public string[] tagsNonBreaking = { "PlayerBullet" };

    private float timeAlive;
    public GameObject shooter;

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
        if (other.gameObject == shooter) return;
        foreach (var tag in tagsNonBreaking)
        {
            if (other.CompareTag(tag)) return;
        }
        int damage = 0;
        //switch (shooter.gameObject)
        //{
        //    case GameObject s when (s.GetComponent<PlayerShoot>() != null):
        //        damage = shooter.gameObject.GetComponent<PlayerShoot>().damage;
        //        break;
        //    case GameObject s when (s.GetComponent<UserPageShoot>() != null):
        //        damage = shooter.gameObject.GetComponent<UserPageShoot>().damage;
        //        break;
        //    case GameObject s when (s.GetComponent<TwoFactorAuthenticationShoot>() != null):
        //        damage = shooter.gameObject.GetComponent<TwoFactorAuthenticationShoot>().damage;
        //        break;
        //    case GameObject s when (s.GetComponent<FirewallShoot>() != null):
        //        damage = shooter.gameObject.GetComponent<FirewallShoot>().damage;
        //        break;
        //    case GameObject s when (s.GetComponent<CapthaShoot>() != null):
        //        damage = shooter.gameObject.GetComponent<CapthaShoot>().damage;
        //        break;
        //    case GameObject s when (s.GetComponent<BackupShoot>() != null):
        //        damage = shooter.gameObject.GetComponent<BackupShoot>().damage;
        //        break;
        //}
        other.gameObject.GetComponent<Health>()?.TakeDamage(shooter.gameObject.GetComponent<Shoot>().damage);
        gameObject.SetActive(false);
    }

    private void OnRenderObject()
    {
    }
}