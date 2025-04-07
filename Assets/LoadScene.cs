using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private System.Collections.IEnumerator LoadNextScene()
    {
        // Ждем один кадр, чтобы дать ProjectContext время на инициализацию
        yield return null;

        // Проверяем наличие ProjectContext в сцене
        var projectContext = FindObjectOfType<ProjectContext>();
        if (projectContext != null)
        {
            Debug.Log("ProjectContext found, loading next scene");
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("ProjectContext not found in BootScene!");
        }
    }
}
