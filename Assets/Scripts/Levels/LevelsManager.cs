using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public CanvasGroup blinkImage; // Ссылка на CanvasGroup, который будет использоваться для моргания
    public float blinkDuration = 0.5f; // Длительность моргания

    public bool isStart;

    public string sceneAfterLevel;
    // Start is called before the first frame update
    void Start()
    {
        if (isStart)
        {
            StartCoroutine(BlinkScene());
        }        
    }

    public IEnumerator BlinkScene()
    {
        // Устанавливаем начальное значение альфа
        blinkImage.alpha = 0;

        yield return Fade(1, 0, blinkDuration);

    }

    public IEnumerator BlinkAndSwitchScene()
    {
        Debug.Log(2);
        // Устанавливаем начальное значение альфа
        blinkImage.alpha = 0;

        // Плавное появление
        yield return Fade(0, 1, blinkDuration);
        yield return Fade(1, 0, blinkDuration);
        yield return Fade(0, 1, blinkDuration);

        // Переключение сцены
        SceneManager.LoadScene(sceneAfterLevel);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            blinkImage.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null; // Ждем следующего кадра
        }

        // Убедимся, что альфа установлена в конечное значение
        blinkImage.alpha = endAlpha;
        blinkImage.interactable = endAlpha > 0; // Устанавливаем интерактивность
        blinkImage.blocksRaycasts = endAlpha > 0;
    }
}

