using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject settingsMenu; // ���������� ������ � ���� ������ � ����������
    public GameObject stopWhenSettings; // ��������� �������� ���� ������� ����
    public GameObject stopFonSound; // ��������� �������� ���� ������� ����

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
        // ��������� ������� ������� Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }
    }
}