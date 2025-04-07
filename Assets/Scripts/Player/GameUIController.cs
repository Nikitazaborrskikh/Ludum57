using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Enemies;
using Zenject;

public class GameUIController : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Image[] healthIcons;
    [SerializeField] private float maxHealth = 6f;

    [Header("Progress Bar")]
    [SerializeField] private Image progressBar;
    [SerializeField] private int totalEnemies;

    [Header("Progress Icon")]
    [SerializeField] private Image progressIcon;
    [SerializeField] private Sprite icon0To50;
    [SerializeField] private Sprite icon50To99;
    [SerializeField] private Sprite icon100;

    [Inject] private PlayerStats playerStats;

    private int remainingEnemies; // Отслеживаем оставшихся врагов

    private void Awake()
    {
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        remainingEnemies = totalEnemies; // Инициализируем счётчик
        Debug.Log($"Initialized with {totalEnemies} enemies in Awake");

        playerStats.OnHealthChanged += UpdateHealthUI;
        foreach (var enemy in FindObjectsOfType<BaseEnemy>())
        {
            enemy.OnEnemyDiedEvent += OnEnemyDied;
            Debug.Log($"Subscribed to Enemy: {enemy.gameObject.name}");
        }

        UpdateHealthUI(playerStats.Health);
        UpdateProgressUI();
    }

    private void OnDestroy()
    {
        playerStats.OnHealthChanged -= UpdateHealthUI;
        foreach (var enemy in FindObjectsOfType<BaseEnemy>())
        {
            enemy.OnEnemyDiedEvent -= OnEnemyDied;
        }
    }

    private void Update()
    {
        UpdateHealthUI(playerStats.Health);
        UpdateProgressUI();
    }

    private void UpdateHealthUI(float health)
    {
        float healthPerIcon = maxHealth / healthIcons.Length;
        float currentHealth = playerStats.Health;

        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].enabled = currentHealth > (i * healthPerIcon);
        }
    }

    private void UpdateProgressUI()
    {
        float progress = totalEnemies > 0 ? (float)(totalEnemies - remainingEnemies) / totalEnemies : 1f;
       

        progressBar.fillAmount = Mathf.Clamp01(progress);

        if (progress >= 1f)
        {
            progressIcon.sprite = icon100;
        }
        else if (progress >= 0.5f)
        {
            progressIcon.sprite = icon50To99;
        }
        else
        {
            progressIcon.sprite = icon0To50;
        }
    }

    private void OnEnemyDied()
    {
        remainingEnemies = Mathf.Max(0, remainingEnemies - 1);
        Debug.Log($"Enemy died, remaining: {remainingEnemies}");
        UpdateProgressUI();
    }
}