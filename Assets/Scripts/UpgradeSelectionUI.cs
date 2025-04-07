using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class UpgradeSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image upgradeImage1;
    [SerializeField] private Image upgradeImage2;
    [SerializeField] private Image nameText1;
    [SerializeField] private Image nameText2;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;

    [Inject] private UpgradeManager upgradeManager;

    private UpgradeSO option1;
    private UpgradeSO option2;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void ShowUpgradeOptions(UpgradeSO opt1, UpgradeSO opt2)
    {
        option1 = opt1;
        option2 = opt2;

        upgradeImage1.sprite = opt1.icon;
        upgradeImage2.sprite = opt2.icon;
        nameText1.sprite = opt1.upgradeNameImg;
        nameText2.sprite = opt2.upgradeNameImg;

        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => SelectUpgrade(option1));
        button2.onClick.AddListener(() => SelectUpgrade(option2));

        panel.SetActive(true);
        Time.timeScale = 0f; // Пауза игры
    }

    private void SelectUpgrade(UpgradeSO upgrade)
    {
        upgradeManager.ApplyUpgrade(upgrade);
        panel.SetActive(false);
        Time.timeScale = 1f; // Возобновление игры
    }
}