using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnHideSettings : MonoBehaviour
{
    public GameObject settingsMenu;
    public AudioClip focusSound;
    public GameObject audioSource;
    public GameObject SettingsMenuController;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(HideSettings);
    }

    private void HideSettings()
    {
        audioSource.GetComponent<AudioSource>().PlayOneShot(focusSound);
        SettingsMenuController.GetComponent<SettingsMenuController>().ToggleSettingsMenu();
    }
}

