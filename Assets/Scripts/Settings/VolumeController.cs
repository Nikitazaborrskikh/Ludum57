using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    // ������ �� ��������� AudioSource
    public AudioSource audioSource;
    public float DefaultVolume = 1f;

    // ������ �� �������
    public Slider volumeSlider;

    void Start()
    {
        // ��������� ���������� �������� ���������
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", DefaultVolume);
        SetVolume(volumeSlider.value);

        // ��������� ��������� ��� ��������� ��������� ��� ����������� ��������
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void OnDisable()
    {
        // ���������� �������� ��������� ��� ������
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    void SetVolume(float volume)
    {
        // ��������� ��������� AudioSource
        audioSource.volume = volume;
    }
}