using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public float DefaultVolume = 1f;

    // Ссылка на слайдер
    public Slider volumeSlider;

    void Start()
    {
        // Установка начального значения громкости
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", DefaultVolume);

        // Добавляем слушатель для изменения громкости при перемещении слайдера
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnDisable()
    {
        // Сохранение значения громкости при выходе
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    public void OnVolumeChanged(float value)
    {
        // Сохранение нового значения чувствительности
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}