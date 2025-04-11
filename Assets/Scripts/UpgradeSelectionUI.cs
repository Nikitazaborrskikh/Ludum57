using System.Collections.Generic;
using System.Linq;
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
    private List<IPausable> pausables = new List<IPausable>();

    private void Awake()
    {
        panel.SetActive(false);
        pausables.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IPausable>());
    }

    public void ShowUpgradeOptions(UpgradeSO opt1, UpgradeSO opt2)
    {
        option1 = opt1;
        option2 = opt2;

  
        upgradeText1.text = opt1.description;
        nameText1.text = opt1.name;
        typeText1.text = opt1.upgradeTypeKey;
        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => SelectUpgrade(option1));
        button1.gameObject.SetActive(true);
        
        if (opt2 != null)
        {
            upgradeText2.text = opt2.description;
            nameText2.text = opt2.name;
            typeText2.text = opt2.upgradeTypeKey;
            button2.onClick.RemoveAllListeners();
            button2.onClick.AddListener(() => SelectUpgrade(option2));
            button2.gameObject.SetActive(true);
        }
        else
        {
          
            upgradeText2.text = "";
            nameText2.text = "";
            typeText2.text = "";
            button2.gameObject.SetActive(false);
        }

        panel.SetActive(true);
        PauseGame();
    }

    private void SelectUpgrade(UpgradeSO upgrade)
    {
        upgradeManager.ApplyUpgrade(upgrade);
        panel.SetActive(false);
        ResumeGame();
    }

    private void PauseGame()
    {
        foreach (var pausable in pausables)
        {
            pausable.Pause();
        }
    }

    private void ResumeGame()
    {
        foreach (var pausable in pausables)
        {
            pausable.Resume();
        }
    }
}

public interface IPausable
{
    void Pause();
    void Resume();
}