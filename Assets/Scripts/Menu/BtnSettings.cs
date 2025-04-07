using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSettings : MonoBehaviour
{
    public GameObject settingsMenu;
    public AudioClip focusSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OpenSettings);
    }

    private void OpenSettings()
    {
        audioSource.PlayOneShot(focusSound);
        settingsMenu.SetActive(true);
    }

}