using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    // Ссылка на компонент AudioSource
    public AudioSource audioSource;
    public float DefaultVolume = 1f;

    // Ссылка на слайдер
    public Slider volumeSlider;

    void Start()
    {
        // Установка начального значения громкости
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", DefaultVolume);
        SetVolume(volumeSlider.value);

        // Добавляем слушатель для изменения громкости при перемещении слайдера
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void OnDisable()
    {
        // Сохранение значения громкости при выходе
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    void SetVolume(float volume)
    {
        // Изменение громкости AudioSource
        audioSource.volume = volume;
    }
}