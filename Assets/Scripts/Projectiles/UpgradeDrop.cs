using UnityEngine;
using Zenject;

public class UpgradeDrop : MonoBehaviour
{
    [Inject] private UpgradeManager upgradeManager;
    [Inject] private UpgradeSelectionUI upgradeSelectionUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            upgradeManager.OfferUpgrades();
            Destroy(gameObject); 
        }
    }
}