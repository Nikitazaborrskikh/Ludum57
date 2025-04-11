using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject primaryBulletPrefab;
    [SerializeField] private GameObject secondaryBulletPrefab;
    [SerializeField] private float bulletSpeed = 10f; // Базовая скорость
    [SerializeField] private Transform bulletSpawnPoint;

    [Inject] private PlayerStats playerStats;
    private float primaryFireTimer;
    private float secondaryFireTimer;
    private bool justDashed;
    private bool isPaused;

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (isPaused) return;
        primaryFireTimer -= Time.deltaTime;
        secondaryFireTimer -= Time.deltaTime;
    }

    public void Shoot(GameObject bulletPrefab, bool isPrimary)
    {
        Vector3 direction = GetShootDirection();
        float currentDamage = playerStats.Damage;
        float currentBulletSpeed = bulletSpeed;

        if (playerStats.SpecialEffects.ContainsKey("SQLi"))
        {
            if (isPrimary && secondaryFireTimer > 0) currentDamage *= 2f;
            if (!isPrimary && primaryFireTimer > 0) currentDamage *= 2f;
        }

        if (playerStats.SpecialEffects.ContainsKey("Zero-Day") && justDashed)
        {
            currentDamage *= 2f;
            justDashed = false;
        }

        if (isPrimary)
        {
            if (playerStats.SpecialEffects.ContainsKey("ConeShot"))
            {
                ShootCone(direction, bulletPrefab, currentDamage);
            }
            else if (playerStats.SpecialEffects.ContainsKey("Ricochet"))
            {
                ShootRicochet(direction, bulletPrefab, currentDamage);
            }
            else
            {
                ShootSingle(direction, bulletPrefab, currentDamage);
            }
            primaryFireTimer = playerStats.FireRate;
        }
        else
        {
            // Устанавливаем множитель скорости только для вторичной атаки
            float speedMultiplier = playerStats.SpecialEffects.ContainsKey("RainbowTables") ? 2f : 1f;
            currentBulletSpeed *= speedMultiplier;
            Debug.Log($"Secondary bullet speed: {currentBulletSpeed} (base: {bulletSpeed}, multiplier: {speedMultiplier})");

            if (playerStats.SpecialEffects.ContainsKey("ParallelShot"))
            {
                ShootParallel(direction, bulletPrefab, currentDamage, currentBulletSpeed);
            }
            else if (playerStats.SpecialEffects.ContainsKey("Sniffing"))
            {
                ShootSniffing(direction, bulletPrefab, currentDamage, currentBulletSpeed);
            }
            else
            {
                ShootSingle(direction, bulletPrefab, currentDamage, null, currentBulletSpeed);
            }
            secondaryFireTimer = playerStats.FireRate;
        }
    }

    private Vector3 GetShootDirection()
    {
        return transform.forward;
    }

    private void ShootSingle(Vector3 direction, GameObject bulletPrefab, float damage, Vector3? spawnPosition = null, float currentBulletSpeed = -1f)
    {
        Vector3 position = spawnPosition ?? bulletSpawnPoint.position;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90f, 0);
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody отсутствует на bulletPrefab!");
            return;
        }

        // Используем переданную скорость или базовую, если не указана
        float finalSpeed = currentBulletSpeed >= 0f ? currentBulletSpeed : bulletSpeed;
        rb.velocity = direction * finalSpeed;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript) bulletScript.SetDamage(damage);
    }

    private void ShootCone(Vector3 direction, GameObject bulletPrefab, float damage)
    {
        Quaternion leftRot = Quaternion.Euler(0, -10, 0);
        Quaternion rightRot = Quaternion.Euler(0, 10, 0);
        ShootSingle(direction, bulletPrefab, damage);
        ShootSingle(leftRot * direction, bulletPrefab, damage);
        ShootSingle(rightRot * direction, bulletPrefab, damage);
    }

    private void ShootRicochet(Vector3 direction, GameObject bulletPrefab, float damage)
    {
        Vector3 position = bulletSpawnPoint.position;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90f, 0);
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null) return;
        rb.velocity = direction * bulletSpeed;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript)
        {
            bulletScript.SetDamage(damage);
            bulletScript.EnableRicochet();
        }
    }

    private void ShootParallel(Vector3 direction, GameObject bulletPrefab, float damage, float currentBulletSpeed)
    {
        Vector3 offset = Vector3.Cross(direction, Vector3.up).normalized * 0.2f;
        ShootSingle(direction, bulletPrefab, damage, bulletSpawnPoint.position + offset, currentBulletSpeed);
        ShootSingle(direction, bulletPrefab, damage, bulletSpawnPoint.position - offset, currentBulletSpeed);
    }

    private void ShootSniffing(Vector3 direction, GameObject bulletPrefab, float damage, float currentBulletSpeed)
    {
        Vector3 position = bulletSpawnPoint.position;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90f, 0);
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null) return;
        rb.velocity = direction * currentBulletSpeed;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript) bulletScript.EnableSniffing();
    }

    private IEnumerator StartSound(AudioClip sound, AudioSource audioSource)
    {
        audioSource.PlayOneShot(sound);
        yield return null;
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context, AudioClip sound, AudioSource audioSource)
    {
        if (context.performed && primaryFireTimer <= 0)
        {
            StartCoroutine(StartSound(sound, audioSource));
            Shoot(primaryBulletPrefab, true);
            if (playerStats.SpecialEffects.ContainsKey("RCE")) Shoot(secondaryBulletPrefab, false);
        }
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context, AudioClip sound, AudioSource audioSource)
    {
        if (context.performed && secondaryFireTimer <= 0)
        {
            StartCoroutine(StartSound(sound, audioSource));
            Shoot(secondaryBulletPrefab, false);
            if (playerStats.SpecialEffects.ContainsKey("RCE")) Shoot(primaryBulletPrefab, true);
        }
    }

    public void SetJustDashed() => justDashed = true;
}