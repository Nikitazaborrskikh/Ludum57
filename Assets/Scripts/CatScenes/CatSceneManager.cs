using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CanvasSwitcher : MonoBehaviour
{
    public CanvasGroup[] canvasGroups; // ������ CanvasGroup ��� ���� ��������
    private int currentCanvasIndex = 0; // ������ �������� �������
    public float fadeDuration = 1f; // ������������ ���������
    public string sceneAfterCatScene;

    public bool isTutorial;

    private bool isAnimation = false;

    void Start()
    {
        // �������� ��� �������, ����� �������
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            if (i == currentCanvasIndex)
            {
                SetCanvasVisibility(i, true);
            }
            else
            {
                SetCanvasVisibility(i, false);
            }
        }
    }

    void Update()
    {
        if ((Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && !isAnimation)
        {
            SwitchCanvas();
        }
    }

    public void SwitchCanvas()
    {
        if (currentCanvasIndex >= canvasGroups.Length-1 && isTutorial)
        {
            return;
        }
        isAnimation = true;
        StartCoroutine(FadeCanvas(currentCanvasIndex, false)); // �������� ������� ������
        currentCanvasIndex++;
        if (currentCanvasIndex >= canvasGroups.Length)
        {
            StartCoroutine(gameObject.GetComponent<LevelsManager>().BlinkAndSwitchScene());
            return;
        }
        StartCoroutine(FadeCanvas(currentCanvasIndex, true)); // �������� ��������� ������
    }

    private IEnumerator FadeCanvas(int index, bool isVisible)
    {
        float startAlpha = isVisible ? 0 : 1;
        float endAlpha = isVisible ? 1 : 0;
        float elapsedTime = 0;

        // ������������� ��������� �������� �����
        canvasGroups[index].alpha = startAlpha;
        canvasGroups[index].interactable = isVisible;
        canvasGroups[index].blocksRaycasts = isVisible;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            canvasGroups[index].alpha = alpha;

            // ������������� ��������������� � ���������� ����� ������ � ����� ���������
            if (!isVisible && elapsedTime >= fadeDuration)
            {
                canvasGroups[index].interactable = false;
                canvasGroups[index].blocksRaycasts = false;
            }
            else if (isVisible && elapsedTime >= fadeDuration)
            {
                canvasGroups[index].interactable = true;
                canvasGroups[index].blocksRaycasts = true;
            }

            yield return null; // ���� ���������� �����
        }

        // ��������, ��� ����� ����������� � �������� ��������
        canvasGroups[index].alpha = endAlpha;
        isAnimation = false;
    }

    private void SetCanvasVisibility(int index, bool isVisible)
    {
        canvasGroups[index].alpha = isVisible ? 1 : 0;
        canvasGroups[index].interactable = isVisible;
        canvasGroups[index].blocksRaycasts = isVisible;
    }
}