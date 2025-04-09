using Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public CanvasGroup blinkImage; // Ссылка на CanvasGroup, который будет использоваться для моргания
    public float blinkDuration = 0.5f; // Длительность моргания

    public bool isStart;
    public bool isBoss;

    public string sceneAfterLevel;
    // Start is called before the first frame update
    private void Start()
    {
        if (isStart)
        {
            StartCoroutine(BlinkScene());
        }        
    }

    private void Update()
    {
        if (isBoss && PlayerPrefs.GetInt("StopLevelsManager", 0) == 0 && !availabilityEnemys())
        {
            StartCoroutine(BlinkAndSwitchScene());
        }
    }

    private bool availabilityEnemys()
    {
        // Получаем все объекты в текущей сцене
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true); // true для поиска неактивных объектов

        List<GameObject> matchingObjects = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Получаем компонент по имени
            Component component = obj.GetComponent<BaseEnemy>();
            if (component != null) 
            { 
                return true;
            }
        }
        return false;
    }

    public IEnumerator BlinkScene()
    {
        // Устанавливаем начальное значение альфа
        blinkImage.alpha = 0;

        yield return Fade(1, 0, blinkDuration);

    }

    private string GetSceneNameByIndex(int index)
    {
        // Проверяем, что индекс находится в пределах допустимого диапазона
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            // Получаем имя сцены по индексу
            return SceneUtility.GetScenePathByBuildIndex(index).Split('/')[3].Replace(".unity", "");
        }
        return null;
    }

    public void BlinkAndSwitchScene(int sceneNum, bool isAvailabilityEnemys)
    {
        if (isAvailabilityEnemys && availabilityEnemys()) return;
        // Устанавливаем начальное значение альфа
        BlinkAndSwitchScene(sceneNum);
    }

    public void BlinkAndSwitchScene(int sceneNum)
    {
        // Устанавливаем начальное значение альфа
        blinkImage.alpha = 0;

        sceneAfterLevel = GetSceneNameByIndex(sceneNum);

        StartCoroutine(fastBlinkAndSwitchScene());
    }

    public void BlinkAndSwitchScene(string sceneName, bool isAvailabilityEnemys)
    {
        if (isAvailabilityEnemys && availabilityEnemys()) return;
        // Устанавливаем начальное значение альфа
        BlinkAndSwitchScene(sceneName);
    }

    public void BlinkAndSwitchScene(string sceneName)
    {
        if (availabilityEnemys()) return;
        // Устанавливаем начальное значение альфа
        blinkImage.alpha = 0;

        sceneAfterLevel = sceneName;

        // Плавное появление
        StartCoroutine(BlinkAndSwitchScene());
    }

    public IEnumerator fastBlinkAndSwitchScene()
    {
        // Устанавливаем начальное значение альфа
        blinkImage.alpha = 0;
        // Плавное появление
        yield return Fade(0, 1, blinkDuration);

        // Переключение сцены
        SceneManager.LoadScene(sceneAfterLevel);
    }

    public IEnumerator BlinkAndSwitchScene()
    {
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

