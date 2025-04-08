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
    public bool isLevel;

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
        if (isLevel && !availabilityEnemys())
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

