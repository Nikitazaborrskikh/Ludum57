using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject settingsMenu; // Перетащите панель в этот объект в инспекторе
    public GameObject stopWhenSettings; // Перестают работать пока открыто меню
    public GameObject stopFonSound; // Перестают работать пока открыто меню

    private void Start()
    {
        PlayerPrefs.SetInt("isSettings", 0);
        settingsMenu.SetActive(false);
    }

    public IEnumerator SetIsSettings()
    {
        yield return new WaitForSeconds(0f);

        PlayerPrefs.SetInt("isSettings", (!settingsMenu.activeSelf) ? 1 : 0);
    }
    public void ToggleSettingsMenu()
    {
        PlayerPrefs.SetInt("isSettings", (!settingsMenu.activeSelf) ? 1 : 0);
        if (stopWhenSettings != null) {stopWhenSettings.SetActive(settingsMenu.activeSelf);}
        if (stopFonSound != null) 
        { 
            if (!settingsMenu.activeSelf) stopFonSound.GetComponent<AudioSource>().Pause(); 
            else stopFonSound.GetComponent<AudioSource>().Play();
        }
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }
    private void Update()
    {
        // Проверяем нажатие клавиши Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }
    }
}