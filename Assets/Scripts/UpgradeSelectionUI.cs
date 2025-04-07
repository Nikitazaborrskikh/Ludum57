using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class UpgradeSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text upgradeText1;
    [SerializeField] private TMP_Text upgradeText2;
    [SerializeField] private TMP_Text nameText1;
    [SerializeField] private TMP_Text nameText2;
    [SerializeField] private TMP_Text typeText1;
    [SerializeField] private TMP_Text typeText2;
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

        upgradeText1.text = opt1.description;
        upgradeText2.text = opt2.description;
        nameText1.text = opt1.name;
        nameText2.text = opt2.name;
        typeText1.text = opt1.upgradeTypeKey;
        typeText2.text = opt2.upgradeTypeKey;
        
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => SelectUpgrade(option1));
        button2.onClick.AddListener(() => SelectUpgrade(option2));

        panel.SetActive(true);
        Time.timeScale = 0f; 
    }

    private void SelectUpgrade(UpgradeSO upgrade)
    {
        upgradeManager.ApplyUpgrade(upgrade);
        panel.SetActive(false);
        Time.timeScale = 1f; 
    }
}

public interface IPausable
{
     void Pause();
     void Resume();
}