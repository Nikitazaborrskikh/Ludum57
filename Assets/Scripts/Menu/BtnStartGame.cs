using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnStartGame : MonoBehaviour
{
    public AudioClip focusSound;
    private AudioSource audioSource;

    public GameObject managers;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        audioSource.PlayOneShot(focusSound);
        StartCoroutine(managers.GetComponent<LevelsManager>().BlinkAndSwitchScene());
    }

}
