using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public float DefaultVolume = 1f;

    // ������ �� �������
    public Slider volumeSlider;

    void Start()
    {
        // ��������� ���������� �������� ���������
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", DefaultVolume);

        // ��������� ��������� ��� ��������� ��������� ��� ����������� ��������
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnDisable()
    {
        // ���������� �������� ��������� ��� ������
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    public void OnVolumeChanged(float value)
    {
        // ���������� ������ �������� ����������������
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}