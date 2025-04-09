using Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public CanvasGroup blinkImage; // ������ �� CanvasGroup, ������� ����� �������������� ��� ��������
    public float blinkDuration = 0.5f; // ������������ ��������

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
        // �������� ��� ������� � ������� �����
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true); // true ��� ������ ���������� ��������

        List<GameObject> matchingObjects = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // �������� ��������� �� �����
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
        // ������������� ��������� �������� �����
        blinkImage.alpha = 0;

        yield return Fade(1, 0, blinkDuration);

    }

    private string GetSceneNameByIndex(int index)
    {
        // ���������, ��� ������ ��������� � �������� ����������� ���������
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            // �������� ��� ����� �� �������
            return SceneUtility.GetScenePathByBuildIndex(index).Split('/')[3].Replace(".unity", "");
        }
        return null;
    }

    public void BlinkAndSwitchScene(int sceneNum, bool isAvailabilityEnemys)
    {
        if (isAvailabilityEnemys && availabilityEnemys()) return;
        // ������������� ��������� �������� �����
        BlinkAndSwitchScene(sceneNum);
    }

    public void BlinkAndSwitchScene(int sceneNum)
    {
        // ������������� ��������� �������� �����
        blinkImage.alpha = 0;

        sceneAfterLevel = GetSceneNameByIndex(sceneNum);

        StartCoroutine(fastBlinkAndSwitchScene());
    }

    public void BlinkAndSwitchScene(string sceneName, bool isAvailabilityEnemys)
    {
        if (isAvailabilityEnemys && availabilityEnemys()) return;
        // ������������� ��������� �������� �����
        BlinkAndSwitchScene(sceneName);
    }

    public void BlinkAndSwitchScene(string sceneName)
    {
        if (availabilityEnemys()) return;
        // ������������� ��������� �������� �����
        blinkImage.alpha = 0;

        sceneAfterLevel = sceneName;

        // ������� ���������
        StartCoroutine(BlinkAndSwitchScene());
    }

    public IEnumerator fastBlinkAndSwitchScene()
    {
        // ������������� ��������� �������� �����
        blinkImage.alpha = 0;
        // ������� ���������
        yield return Fade(0, 1, blinkDuration);

        // ������������ �����
        SceneManager.LoadScene(sceneAfterLevel);
    }

    public IEnumerator BlinkAndSwitchScene()
    {
        // ������������� ��������� �������� �����
        blinkImage.alpha = 0;

        // ������� ���������
        yield return Fade(0, 1, blinkDuration);
        yield return Fade(1, 0, blinkDuration);
        yield return Fade(0, 1, blinkDuration);

        // ������������ �����
        SceneManager.LoadScene(sceneAfterLevel);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            blinkImage.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null; // ���� ���������� �����
        }

        // ��������, ��� ����� ����������� � �������� ��������
        blinkImage.alpha = endAlpha;
        blinkImage.interactable = endAlpha > 0; // ������������� ���������������
        blinkImage.blocksRaycasts = endAlpha > 0;
    }
}

