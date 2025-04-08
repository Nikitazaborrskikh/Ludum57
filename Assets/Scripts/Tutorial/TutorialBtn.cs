using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialBtn : MonoBehaviour
{
    public GameObject managers;
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        StartCoroutine(managers.GetComponent<LevelsManager>().BlinkAndSwitchScene());
    }
}
