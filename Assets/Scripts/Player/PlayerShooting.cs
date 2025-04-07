using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject primaryBulletPrefab;
    [SerializeField] private GameObject secondaryBulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
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
        float currentBulletSpeed = bulletSpeed; // Локальная копия

        if (playerStats.SpecialEffects.ContainsKey("SQLi"))
        {
            if (isPrimary && secondaryFireTimer > 0) currentDamage *= 1.5f;
            if (!isPrimary && primaryFireTimer > 0) currentDamage *= 1.5f;
        }
    
        if (playerStats.SpecialEffects.ContainsKey("Zero-Day") && justDashed)
        {
            currentDamage *= 1.5f;
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
            currentBulletSpeed = playerStats.SpecialEffects.ContainsKey("RainbowTables") ? currentBulletSpeed * 1.5f : currentBulletSpeed;
            if (playerStats.SpecialEffects.ContainsKey("ParallelShot"))
            {
                ShootParallel(direction, bulletPrefab, currentDamage);
            }
            else if (playerStats.SpecialEffects.ContainsKey("Sniffing"))
            {
                ShootSniffing(direction, bulletPrefab, currentDamage);
            }
            else
            {
                ShootSingle(direction, bulletPrefab, currentDamage);
            }
            secondaryFireTimer = playerStats.FireRate;
        }
    }

    private Vector3 GetShootDirection()
    {
        // Vector2 mousePosition = Mouse.current.position.ReadValue();
        // Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        // Plane plane = new Plane(Vector3.up, transform.position);
        // float distance;
        // plane.Raycast(ray, out distance);
        // Vector3 targetPoint = ray.GetPoint(distance);
        // targetPoint.y = bulletSpawnPoint.position.y;
        // return (targetPoint - bulletSpawnPoint.position).normalized;
        return transform.forward;
    }

    private void ShootSingle(Vector3 direction, GameObject bulletPrefab, float damage, Vector3? spawnPosition = null)
    {
        Vector3 position = spawnPosition ?? bulletSpawnPoint.position;
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
    
        if (rb == null)
        {
            Debug.LogError("Rigidbody отсутствует на bulletPrefab!");
            return;
        }

        // Проверка значений перед установкой velocity
        Debug.Log($"ShootSingle - Position: {position}, Direction: {direction}, BulletSpeed: {bulletSpeed}, Velocity: {direction * bulletSpeed}");
    
        rb.velocity = direction * bulletSpeed;

        // Проверка velocity после установки
        Debug.Log($"Velocity после установки: {rb.velocity}");

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
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody отсутствует на bulletPrefab!");
            return;
        }
        rb.velocity = direction * bulletSpeed;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript)
        {
            bulletScript.SetDamage(damage);
            bulletScript.EnableRicochet();
        }
    }

    private void ShootParallel(Vector3 direction, GameObject bulletPrefab, float damage)
    {
        Vector3 offset = Vector3.Cross(direction, Vector3.up).normalized * 0.2f;
        ShootSingle(direction, bulletPrefab, damage, bulletSpawnPoint.position + offset);
        ShootSingle(direction, bulletPrefab, damage, bulletSpawnPoint.position - offset);
    }

    private void ShootSniffing(Vector3 direction, GameObject bulletPrefab, float damage)
    {
        Vector3 position = bulletSpawnPoint.position;
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody отсутствует на bulletPrefab!");
            return;
        }
        rb.velocity = direction * bulletSpeed;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript) bulletScript.EnableSniffing();
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed && primaryFireTimer <= 0)
        {
            Shoot(primaryBulletPrefab, true);
            if (playerStats.SpecialEffects.ContainsKey("RCE")) Shoot(secondaryBulletPrefab, false); 
        }
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed && secondaryFireTimer <= 0)
        {
            Shoot(secondaryBulletPrefab, false);
            if (playerStats.SpecialEffects.ContainsKey("RCE")) Shoot(primaryBulletPrefab, true);
        }
    }

    public void SetJustDashed() => justDashed = true; 
}