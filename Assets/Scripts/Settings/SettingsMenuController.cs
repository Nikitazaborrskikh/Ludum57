using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject settingsMenu; // ���������� ������ � ���� ������ � ����������

    private void Start()
    {
        settingsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ToggleSettingsMenu()
    {
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