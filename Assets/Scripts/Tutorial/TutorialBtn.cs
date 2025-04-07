using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialBtn : MonoBehaviour
{
    public AudioClip focusSound;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        audioSource.PlayOneShot(focusSound);
        // SceneManager.LoadScene("TestSceneShoot");
    }
}
