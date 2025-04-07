using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    public Slider sensitivitySlider; // Ссылка на Slider в UI
    public float DefaultSensitivity = 2.0f; // Значение по умолчанию

    void Start()
    {
        // Установка начального значения чувствительности
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", DefaultSensitivity);

        // Добавляем слушатель для изменения чувствительности при перемещении слайдера
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    void OnDisable()
    {
        // Сохранение значения чувствительности при выходе
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);
    }

    // Метод, изменяющий чувствительность мыши
    public void OnSensitivityChanged(float value)
    {
        // Сохранение нового значения чувствительности
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
    }
}