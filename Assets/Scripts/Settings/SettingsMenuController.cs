using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject settingsMenu; // Перетащите панель в этот объект в инспекторе
    public GameObject stopWhenSettings; // Перестают работать пока открыто меню
    public GameObject stopFonSound; // Перестают работать пока открыто меню

    private void Start()
    {
        settingsMenu.SetActive(false);
    }
    public void ToggleSettingsMenu()
    {
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