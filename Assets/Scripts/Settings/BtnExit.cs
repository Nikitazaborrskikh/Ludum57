using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnExit : MonoBehaviour
{

    void Start()
    {
        // Получаем доступ к компоненту Button и добавляем обработчик события
        Button button = GetComponent<Button>();
        button.onClick.AddListener(CloseGame);
    }

    // Метод для закрытия меню
    public void CloseGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

