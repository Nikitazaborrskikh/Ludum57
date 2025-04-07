using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSettings : MonoBehaviour
{
    public GameObject settingsMenu;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OpenSettings);
    }

    private void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

}