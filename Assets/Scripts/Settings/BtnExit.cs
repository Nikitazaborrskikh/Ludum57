using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnExit : MonoBehaviour
{

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(CloseGame);
    }

    public void CloseGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

